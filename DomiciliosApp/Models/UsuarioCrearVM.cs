using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DomiciliosApp.Models;

/// <summary>
/// Formulario único para crear un usuario y, según su tipo, su fila en cliente y/o domiciliario.
/// Lo usan Usuarios/Create, Clientes/Create y Domiciliarios/Create.
/// </summary>
public class UsuarioCrearVM : IValidatableObject
{
    public const string TipoCliente = "cliente";
    public const string TipoDomiciliario = "domiciliario";
    public const string TipoAmbos = "ambos";

    // Datos personales: reutilizan las reglas de Metadata.cs (documento, nombre, teléfono, correo)
    public usuario Usuario { get; set; } = new usuario();

    [Display(Name = "Tipo de usuario")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public string? Tipo { get; set; }

    // Datos extra: solo se exigen si el tipo incluye domiciliario (ver Validate)
    [Display(Name = "Código")]
    [Range(1, 999999, ErrorMessage = "El código debe ser un número entre 1 y 999999.")]
    public int? Codigo { get; set; }

    [Display(Name = "Medio de transporte")]
    public int? IdTransporte { get; set; }

    [Display(Name = "Estado")]
    public int? IdEstado { get; set; }

    public bool EsCliente => Tipo == TipoCliente || Tipo == TipoAmbos;
    public bool EsDomiciliario => Tipo == TipoDomiciliario || Tipo == TipoAmbos;

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        if (Tipo != TipoCliente && Tipo != TipoDomiciliario && Tipo != TipoAmbos)
        {
            yield return new ValidationResult("Seleccione el tipo de usuario.", new[] { nameof(Tipo) });
            yield break;
        }

        if (!EsDomiciliario) yield break;

        if (Codigo == null)
            yield return new ValidationResult("El código es obligatorio para un domiciliario.", new[] { nameof(Codigo) });
        else if (ctx.GetService(typeof(DomiciliosContext)) is DomiciliosContext db &&
                 db.domiciliarios.Any(d => d.codigo == Codigo))
            yield return new ValidationResult("Ya existe un domiciliario con ese código.", new[] { nameof(Codigo) });

        if (IdTransporte == null)
            yield return new ValidationResult("Seleccione el medio de transporte.", new[] { nameof(IdTransporte) });

        if (IdEstado == null)
            yield return new ValidationResult("Seleccione el estado.", new[] { nameof(IdEstado) });
    }
}

public static class UsuarioServicio
{
    /// <summary>
    /// Guarda el usuario con todos sus datos personales y, según el tipo, inserta en cliente y/o
    /// domiciliario. El ID lo genera la base de datos (AUTO_INCREMENT); al enlazar por la propiedad de
    /// navegación, EF copia ese ID a cliente / domiciliario. Un solo SaveChanges = todo o nada.
    /// </summary>
    public static async Task CrearAsync(DomiciliosContext db, UsuarioCrearVM vm)
    {
        vm.Usuario.id_usuario = 0;
        db.usuarios.Add(vm.Usuario);

        if (vm.EsCliente)
            db.clientes.Add(new cliente { id_clienteNavigation = vm.Usuario });

        if (vm.EsDomiciliario)
            db.domiciliarios.Add(new domiciliario
            {
                id_domiciliarioNavigation = vm.Usuario,
                codigo = vm.Codigo!.Value,
                id_transporte = vm.IdTransporte!.Value,
                id_estado = vm.IdEstado!.Value
            });

        await db.SaveChangesAsync();
    }
}
