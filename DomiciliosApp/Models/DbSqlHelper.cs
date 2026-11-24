using System.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DomiciliosApp.Models;

public static class DbSqlHelper
{
    /// <summary>
    /// Extrae el MESSAGE_TEXT cuando MySQL lanza SIGNAL SQLSTATE '45000'
    /// desde un trigger o un procedimiento almacenado.
    /// </summary>
    public static string? ExtraerMensajeNegocio(Exception ex)
    {
        var actual = ex;
        while (actual != null)
        {
            if (actual is MySqlException mysqlEx &&
                (mysqlEx.SqlState == "45000" || mysqlEx.Number == 1644))
            {
                return mysqlEx.Message;
            }
            actual = actual.InnerException;
        }
        return null;
    }

    // ==================== FUNCIONES (functions.sql) ====================

    public static async Task<decimal> ObtenerTotalClienteAsync(DomiciliosContext db, int idCliente)
    {
        return await db.Database
            .SqlQuery<decimal>($"SELECT fn_total_cliente({idCliente}) AS Value")
            .SingleAsync();
    }

    public static async Task<bool> DomiciliarioDisponibleAsync(DomiciliosContext db, int idDomiciliario)
    {
        var disponible = await db.Database
            .SqlQuery<int>($"SELECT CAST(fn_domiciliario_disponible({idDomiciliario}) AS SIGNED) AS Value")
            .SingleAsync();
        return disponible == 1;
    }

    public static async Task<int?> ObtenerMinutosAsignacionAsync(DomiciliosContext db, int idSolicitud)
    {
        return await db.Database
            .SqlQuery<int?>($"SELECT fn_minutos_asignacion({idSolicitud}) AS Value")
            .SingleAsync();
    }

    // ================= PROCEDIMIENTOS (procedures.sql) =================

    public static async Task<int> CrearSolicitudAsync(DomiciliosContext db, solicitud s)
    {
        var conn = db.Database.GetDbConnection();
        var abrirAqui = conn.State != ConnectionState.Open;
        if (abrirAqui)
            await conn.OpenAsync();

        try
        {
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_crear_solicitud";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("p_cliente", s.id_cliente));
            cmd.Parameters.Add(new MySqlParameter("p_direccionEntrega", s.id_direccionEntrega));
            cmd.Parameters.Add(new MySqlParameter("p_metodoPago", s.id_metodoPago));
            cmd.Parameters.Add(new MySqlParameter("p_monto", s.montoTotal));
            cmd.Parameters.Add(new MySqlParameter("p_descripcion", (object?)s.descripcion ?? DBNull.Value));
            cmd.Parameters.Add(new MySqlParameter("p_establecimiento", (object?)s.id_establecimientoRecogida ?? DBNull.Value));
            cmd.Parameters.Add(new MySqlParameter("p_direccionExterna", (object?)s.id_direccionRecogidaExterna ?? DBNull.Value));

            var pIdOut = new MySqlParameter("p_id", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(pIdOut);

            await cmd.ExecuteNonQueryAsync();
            return Convert.ToInt32(pIdOut.Value);
        }
        finally
        {
            if (abrirAqui)
                await conn.CloseAsync();
        }
    }
    public static async Task AsignarDomiciliarioAsync(DomiciliosContext db, int idSolicitud, int idDomiciliario)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_asignar_domiciliario({idSolicitud}, {idDomiciliario})");
    }

    public static async Task FinalizarSolicitudAsync(DomiciliosContext db, int idSolicitud)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_finalizar_solicitud({idSolicitud})");
    }

    public static async Task CancelarSolicitudAsync(DomiciliosContext db, int idSolicitud)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_cancelar_solicitud({idSolicitud})");
    }

    public static async Task RegistrarIncidenteAsync(DomiciliosContext db, int idSolicitud, int idTipo, string? descripcion)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_registrar_incidente({idSolicitud}, {idTipo}, {descripcion})");
    }

    public static async Task IniciarEntregaAsync(DomiciliosContext db, int idSolicitud)
    {
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"CALL sp_iniciar_entrega({idSolicitud})");
    }
}