using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DomiciliosApp.Models;

/// <summary>
/// Listas para los desplegables (id + texto legible) y utilidades de formato.
/// Las vistas las usan con: @inject DomiciliosContext Db  ->  Db.Estados(), Db.Clientes(), etc.
/// </summary>
public static class Opciones
{
    private static SelectListItem Item(int id, string? texto) =>
        new SelectListItem(string.IsNullOrWhiteSpace(texto) ? $"#{id}" : texto, id.ToString(CultureInfo.InvariantCulture));

    private static string Cod(object? codigo) => codigo is null ? "" : $" (cód. {codigo})";

    private static string Dir(string? ubicacion, string? barrio) =>
        string.IsNullOrWhiteSpace(barrio) ? (ubicacion ?? "") : $"{ubicacion} · {barrio}";

    // ---------- Catálogos ----------
    public static List<SelectListItem> Estados(this DomiciliosContext db) =>
        db.estadosolicituds.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_estado, x.nombre)).ToList();

    public static List<SelectListItem> Metodos(this DomiciliosContext db) =>
        db.metodopagos.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_metodoPago, x.nombre)).ToList();

    public static List<SelectListItem> TiposIncidente(this DomiciliosContext db) =>
        db.tipoincidentes.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_tipoIncidente, x.nombre)).ToList();

    public static List<SelectListItem> TiposEstablecimiento(this DomiciliosContext db) =>
        db.tipoestablecimientos.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_tipoEstablecimiento, x.nombre)).ToList();

    public static List<SelectListItem> EstadosDomiciliario(this DomiciliosContext db) =>
        db.estadodomiciliarios.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_estado, x.nombre)).ToList();

    public static List<SelectListItem> Transportes(this DomiciliosContext db) =>
        db.mediodetransportes.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_transporte, x.nombre)).ToList();

    // ---------- Direcciones ----------
    public static List<SelectListItem> Direcciones(this DomiciliosContext db) =>
        db.direccions.AsNoTracking().OrderBy(x => x.ubicacion).ToList()
          .Select(x => Item(x.id_direccion, Dir(x.ubicacion, x.barrio))).ToList();

    /// <summary>Direcciones registradas por cada cliente (tabla cliente_direccion).</summary>
    public static Dictionary<int, List<SelectListItem>> DireccionesPorCliente(this DomiciliosContext db)
    {
        var q = (from cd in db.cliente_direccions
                 join d in db.direccions on cd.id_direccion equals d.id_direccion
                 select new { cd.id_cliente, d.id_direccion, d.ubicacion, d.barrio })
                .AsNoTracking().ToList();

        return q.GroupBy(x => x.id_cliente)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.ubicacion)
                          .Select(x => Item(x.id_direccion, Dir(x.ubicacion, x.barrio))).ToList());
    }

    /// <summary>JSON seguro (escapa &lt; &gt; &amp;) para pasarlo a JavaScript.</summary>
    public static string AJson(this Dictionary<int, List<SelectListItem>> mapa) =>
        JsonSerializer.Serialize(mapa.ToDictionary(
            k => k.Key.ToString(CultureInfo.InvariantCulture),
            k => k.Value.Select(i => new { value = i.Value, text = i.Text }).ToList()));

    // ---------- Personas ----------
    private static string Persona(string? nombre, string? doc) => $"{nombre} — {doc}";

    public static List<SelectListItem> Usuarios(this DomiciliosContext db) =>
        db.usuarios.AsNoTracking().OrderBy(x => x.nombre_completo).ToList()
          .Select(x => Item(x.id_usuario, Persona(x.nombre_completo, x.numeroId))).ToList();

    /// <summary>Usuarios que todavía no son clientes (para crear un cliente).</summary>
    public static List<SelectListItem> UsuariosParaCliente(this DomiciliosContext db)
    {
        var ya = db.clientes.AsNoTracking().Select(c => c.id_cliente).ToList();
        return db.usuarios.AsNoTracking().OrderBy(x => x.nombre_completo).ToList()
                 .Where(x => !ya.Contains(x.id_usuario))
                 .Select(x => Item(x.id_usuario, Persona(x.nombre_completo, x.numeroId))).ToList();
    }

    /// <summary>Usuarios que todavía no son domiciliarios.</summary>
    public static List<SelectListItem> UsuariosParaDomiciliario(this DomiciliosContext db)
    {
        var ya = db.domiciliarios.AsNoTracking().Select(d => d.id_domiciliario).ToList();
        return db.usuarios.AsNoTracking().OrderBy(x => x.nombre_completo).ToList()
                 .Where(x => !ya.Contains(x.id_usuario))
                 .Select(x => Item(x.id_usuario, Persona(x.nombre_completo, x.numeroId))).ToList();
    }

    public static List<SelectListItem> Clientes(this DomiciliosContext db)
    {
        var q = (from c in db.clientes
                 join u in db.usuarios on c.id_cliente equals u.id_usuario
                 select new { c.id_cliente, u.nombre_completo, u.numeroId }).AsNoTracking().ToList();
        return q.OrderBy(x => x.nombre_completo)
                .Select(x => Item(x.id_cliente, Persona(x.nombre_completo, x.numeroId))).ToList();
    }

    public static List<SelectListItem> Domiciliarios(this DomiciliosContext db)
    {
        var q = (from d in db.domiciliarios
                 join u in db.usuarios on d.id_domiciliario equals u.id_usuario
                 select new { d.id_domiciliario, d.codigo, u.nombre_completo }).AsNoTracking().ToList();
        return q.OrderBy(x => x.nombre_completo)
                .Select(x => Item(x.id_domiciliario, $"{x.nombre_completo}{Cod(x.codigo)}")).ToList();
    }

    public static List<SelectListItem> Establecimientos(this DomiciliosContext db) =>
        db.establecimientos.AsNoTracking().OrderBy(x => x.nombre).ToList()
          .Select(x => Item(x.id_establecimiento, $"{x.nombre}{Cod(x.codigo)}")).ToList();

    public static List<SelectListItem> Solicitudes(this DomiciliosContext db)
    {
        var q = (from s in db.solicituds
                 join u in db.usuarios on s.id_cliente equals u.id_usuario
                 select new { s.id_solicitud, u.nombre_completo, s.fechaCreacion }).AsNoTracking().ToList();
        return q.OrderByDescending(x => x.id_solicitud)
                .Select(x => Item(x.id_solicitud, $"#{x.id_solicitud} — {x.nombre_completo} — {x.fechaCreacion.Mostrar()}")).ToList();
    }

    // ---------- Utilidades para las vistas ----------
    /// <summary>Devuelve el texto de la opción cuyo valor es "id" (o "—" si no existe).</summary>
    public static string Texto(this IEnumerable<SelectListItem> lista, object? id) =>
        id is null
            ? "—"
            : lista.FirstOrDefault(i => i.Value == Convert.ToString(id, CultureInfo.InvariantCulture))?.Text ?? "—";

    /// <summary>Formato de presentación: fechas dd/MM/yyyy, horas HH:mm, dinero $ 1.500,00, vacío como "—".</summary>
    public static string Mostrar(this object? v)
    {
        var co = CultureInfo.GetCultureInfo("es-CO");
        return v switch
        {
            null => "—",
            string s => string.IsNullOrWhiteSpace(s) ? "—" : s,
            DateOnly d => d.ToString("dd/MM/yyyy"),
            TimeOnly t => t.ToString("HH:mm"),
            TimeSpan ts => ts.ToString(@"hh\:mm"),
            DateTime dt => dt.ToString("dd/MM/yyyy HH:mm"),
            decimal m => "$ " + m.ToString("N2", co),
            _ => Convert.ToString(v, CultureInfo.InvariantCulture) ?? "—"
        };
    }
}
