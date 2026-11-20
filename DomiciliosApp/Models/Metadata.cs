using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DomiciliosApp.Models;

// ============================================================================
//  Reglas de validación y nombres de campo (etiquetas).
//  Viven AQUÍ y no en los modelos generados, así no se pierden si vuelves a
//  ejecutar "dotnet ef dbcontext scaffold". Requiere que las clases generadas
//  sean "partial" (lo son por defecto).
//
//  Las expresiones regulares evitan \p{L} a propósito: JavaScript (validación
//  del navegador) no lo entiende sin la bandera "u". Por eso las letras con
//  tilde se listan de forma explícita.
// ============================================================================

internal static class Reglas
{
    public const string Letras = "A-Za-zÁÉÍÓÚÜÑáéíóúüñ";

    // Nombre de persona: letras, espacios, apóstrofo, punto y guion
    public const string NombrePersona = "^[" + Letras + "][" + Letras + @"\s'.\-]*$";

    // Nombre de catálogo: letras, números, espacios y . , / ( ) -
    public const string NombreCatalogo = "^[" + Letras + "0-9][" + Letras + @"0-9\s.,/()\-]*$";

    // Dirección: letras, números, espacios y # . , ° / ( ) -   (ej. "Cra 5 # 12-34 Apto 301")
    public const string Direccion = "^[" + Letras + "0-9][" + Letras + @"0-9\s#.,°º/()\-]*$";

    // Barrio / comuna
    public const string Lugar = "^[" + Letras + "0-9][" + Letras + @"0-9\s.,()\-]*$";

    // Celular colombiano (3XXXXXXXXX) o fijo con indicativo (60X + 7 dígitos)
    public const string Telefono = @"^(3\d{9}|60[1-8]\d{7})$";

    public const string Documento = @"^\d{5,20}$";

    public const string Correo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    // Dinero: hasta 8 enteros y 2 decimales, con punto como separador
    public const string Dinero = @"^\d{1,8}(\.\d{1,2})?$";

    public const string Obligatorio = "{0} es obligatorio.";
    public const string Elegir = "Seleccione {0}.";
    public const string IdPositivo = "El ID debe ser un número entero mayor que 0.";
}


internal static class Unicos
{
    public static DomiciliosContext? Db(ValidationContext c) => c.GetService(typeof(DomiciliosContext)) as DomiciliosContext;
}

// ---------------------------------------------------------------- usuario
[ModelMetadataType(typeof(usuarioMeta))]
public partial class usuario : IValidatableObject
{
    // numeroId tiene índice UNIQUE en la base de datos
    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        var db = Unicos.Db(ctx);
        if (db != null && db.usuarios.Any(u => u.numeroId == numeroId && u.id_usuario != id_usuario))
            yield return new ValidationResult("Ya existe un usuario con ese documento.", new[] { nameof(numeroId) });
    }
}

public class usuarioMeta
{
    [Display(Name = "ID")]
    public object? id_usuario { get; set; }

    [Display(Name = "Documento de identidad")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [RegularExpression(Reglas.Documento, ErrorMessage = "El documento debe tener solo números (entre 5 y 20 dígitos).")]
    public object? numeroId { get; set; }

    [Display(Name = "Nombre completo")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 255 caracteres.")]
    [RegularExpression(Reglas.NombrePersona, ErrorMessage = "El nombre solo puede tener letras, espacios, punto, apóstrofo y guion.")]
    public object? nombre_completo { get; set; }

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [RegularExpression(Reglas.Telefono, ErrorMessage = "Escriba un celular de 10 dígitos (ej. 3001234567) o un fijo con indicativo (ej. 6021234567).")]
    public object? telefono { get; set; }

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(255, ErrorMessage = "El correo no puede superar 255 caracteres.")]
    [EmailAddress(ErrorMessage = "Escriba un correo válido (ej. nombre@dominio.com).")]
    [RegularExpression(Reglas.Correo, ErrorMessage = "Escriba un correo válido (ej. nombre@dominio.com).")]
    public object? correo { get; set; }
}

// ---------------------------------------------------------------- cliente
[ModelMetadataType(typeof(clienteMeta))]
public partial class cliente { }

public class clienteMeta
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_cliente { get; set; }
}

// ---------------------------------------------------------------- domiciliario
[ModelMetadataType(typeof(domiciliarioMeta))]
public partial class domiciliario : IValidatableObject
{
    // codigo tiene índice UNIQUE en la base de datos
    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        var db = Unicos.Db(ctx);
        if (db != null && codigo != null && db.domiciliarios.Any(d => d.codigo == codigo && d.id_domiciliario != id_domiciliario))
            yield return new ValidationResult("Ya existe un domiciliario con ese código.", new[] { nameof(codigo) });
    }
}

public class domiciliarioMeta
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_domiciliario { get; set; }

    [Display(Name = "Código")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [Range(1, 999999, ErrorMessage = "El código debe ser un número entre 1 y 999999.")]
    public object? codigo { get; set; }

    [Display(Name = "Medio de transporte")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_transporte { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_estado { get; set; }
}

// ---------------------------------------------------------------- establecimiento
[ModelMetadataType(typeof(establecimientoMeta))]
public partial class establecimiento : IValidatableObject
{
    // codigo tiene índice UNIQUE en la base de datos
    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        var db = Unicos.Db(ctx);
        if (db != null && codigo != null && db.establecimientos.Any(e => e.codigo == codigo && e.id_establecimiento != id_establecimiento))
            yield return new ValidationResult("Ya existe un establecimiento con ese código.", new[] { nameof(codigo) });
    }
}

public class establecimientoMeta
{
    [Display(Name = "ID")]
    public object? id_establecimiento { get; set; }

    [Display(Name = "Código")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [Range(1, 999999, ErrorMessage = "El código debe ser un número entre 1 y 999999.")]
    public object? codigo { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }

    [Display(Name = "Tipo de establecimiento")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_tipoEstablecimiento { get; set; }

    [Display(Name = "Dirección")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_direccion { get; set; }

    [Display(Name = "Teléfono")]
    [RegularExpression(Reglas.Telefono, ErrorMessage = "Escriba un celular de 10 dígitos (ej. 3001234567) o un fijo con indicativo (ej. 6021234567).")]
    public object? telefono { get; set; }

    [Display(Name = "Correo electrónico")]
    [StringLength(100, ErrorMessage = "El correo no puede superar 100 caracteres.")]
    [EmailAddress(ErrorMessage = "Escriba un correo válido (ej. nombre@dominio.com).")]
    [RegularExpression(Reglas.Correo, ErrorMessage = "Escriba un correo válido (ej. nombre@dominio.com).")]
    public object? email { get; set; }
}

// ---------------------------------------------------------------- direccion
[ModelMetadataType(typeof(direccionMeta))]
public partial class direccion { }

public class direccionMeta
{
    [Display(Name = "ID")]
    public object? id_direccion { get; set; }

    [Display(Name = "Dirección")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 500 caracteres.")]
    [RegularExpression(Reglas.Direccion, ErrorMessage = "La dirección solo puede tener letras, números, espacios y # . , ° / ( ) -")]
    public object? ubicacion { get; set; }

    [Display(Name = "Barrio")]
    [StringLength(255, ErrorMessage = "El barrio no puede superar 255 caracteres.")]
    [RegularExpression(Reglas.Lugar, ErrorMessage = "El barrio solo puede tener letras, números, espacios y . , ( ) -")]
    public object? barrio { get; set; }

    [Display(Name = "Comuna")]
    [StringLength(255, ErrorMessage = "La comuna no puede superar 255 caracteres.")]
    [RegularExpression(Reglas.Lugar, ErrorMessage = "La comuna solo puede tener letras, números, espacios y . , ( ) -")]
    public object? comuna { get; set; }

    [Display(Name = "Referencia")]
    [StringLength(500, ErrorMessage = "La referencia no puede superar 500 caracteres.")]
    public object? referencia { get; set; }
}

// ---------------------------------------------------------------- cliente_direccion
[ModelMetadataType(typeof(cliente_direccionMeta))]
public partial class cliente_direccion : IValidatableObject
{
    // La clave primaria es (cliente, dirección): no se puede repetir la pareja
    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        var db = Unicos.Db(ctx);
        if (db != null && db.cliente_direccions.Any(x => x.id_cliente == id_cliente && x.id_direccion == id_direccion))
            yield return new ValidationResult("Esa dirección ya está registrada para el cliente.", new[] { nameof(id_direccion) });
    }
}

public class cliente_direccionMeta
{
    [Display(Name = "Cliente")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_cliente { get; set; }

    [Display(Name = "Dirección")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_direccion { get; set; }
}

// ---------------------------------------------------------------- catálogos (id + nombre)
[ModelMetadataType(typeof(estadosolicitudMeta))]
public partial class estadosolicitud { }
public class estadosolicitudMeta
{
    [Display(Name = "ID")]
    public object? id_estado { get; set; }

    [Display(Name = "Nombre del estado")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

[ModelMetadataType(typeof(estadodomiciliarioMeta))]
public partial class estadodomiciliario { }
public class estadodomiciliarioMeta
{
    [Display(Name = "ID")]
    public object? id_estado { get; set; }

    [Display(Name = "Nombre del estado")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

[ModelMetadataType(typeof(mediodetransporteMeta))]
public partial class mediodetransporte { }
public class mediodetransporteMeta
{
    [Display(Name = "ID")]
    public object? id_transporte { get; set; }

    [Display(Name = "Nombre del medio de transporte")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 255 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

[ModelMetadataType(typeof(metodopagoMeta))]
public partial class metodopago { }
public class metodopagoMeta
{
    [Display(Name = "ID")]
    public object? id_metodoPago { get; set; }

    [Display(Name = "Nombre del método de pago")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 255 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

[ModelMetadataType(typeof(tipoestablecimientoMeta))]
public partial class tipoestablecimiento { }
public class tipoestablecimientoMeta
{
    [Display(Name = "ID")]
    public object? id_tipoEstablecimiento { get; set; }

    [Display(Name = "Nombre del tipo")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 255 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

[ModelMetadataType(typeof(tipoincidenteMeta))]
public partial class tipoincidente { }
public class tipoincidenteMeta
{
    [Display(Name = "ID")]
    public object? id_tipoIncidente { get; set; }

    [Display(Name = "Nombre del tipo")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [RegularExpression(Reglas.NombreCatalogo, ErrorMessage = "El nombre solo puede tener letras, números, espacios y . , / ( ) -")]
    public object? nombre { get; set; }
}

// ---------------------------------------------------------------- incidente
[ModelMetadataType(typeof(incidenteMeta))]
public partial class incidente { }

public class incidenteMeta
{
    [Display(Name = "ID")]
    public object? id_incidente { get; set; }

    [Display(Name = "Tipo de incidente")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? tipoIncidente { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 100 caracteres.")]
    public object? descripcion { get; set; }

    [Display(Name = "Fecha")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [DataType(DataType.Date)]
    public object? fecha { get; set; }

    [Display(Name = "Hora")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [DataType(DataType.Time)]
    public object? hora { get; set; }

    [Display(Name = "Solicitud")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_solicitud { get; set; }
}

// ---------------------------------------------------------------- solicitud
[ModelMetadataType(typeof(solicitudMeta))]
public partial class solicitud : IValidatableObject
{
    // Reglas que cruzan varios campos. Se ejecutan en el servidor después de las reglas de cada campo.
    public IEnumerable<ValidationResult> Validate(ValidationContext contexto)
    {
        // 1) Recogida: un establecimiento O una dirección externa (no ambos, no ninguno).
        //    Si en tu negocio se permite no indicar ninguno, borra el primer "if".
        if (id_establecimientoRecogida == null && id_direccionRecogidaExterna == null)
            yield return new ValidationResult(
                "Indique un establecimiento o una dirección externa de recogida.",
                new[] { nameof(id_establecimientoRecogida) });

        if (id_establecimientoRecogida != null && id_direccionRecogidaExterna != null)
            yield return new ValidationResult(
                "Elija solo uno: establecimiento o dirección externa.",
                new[] { nameof(id_direccionRecogidaExterna) });

        // 2) Asignación: domiciliario, fecha y hora van juntos.
        if (id_domiciliario != null && (fechaAsignacion == null || horaAsignacion == null))
            yield return new ValidationResult(
                "Si hay domiciliario, indique la fecha y la hora de asignación.",
                new[] { nameof(fechaAsignacion) });

        if (id_domiciliario == null && (fechaAsignacion != null || horaAsignacion != null))
            yield return new ValidationResult(
                "La fecha y hora de asignación solo aplican si hay un domiciliario asignado.",
                new[] { nameof(id_domiciliario) });

        // 3) La asignación no puede ser anterior a la creación.
        if (fechaAsignacion != null && fechaAsignacion < fechaCreacion)
            yield return new ValidationResult(
                "La fecha de asignación no puede ser anterior a la de creación.",
                new[] { nameof(fechaAsignacion) });

        // 4) Máximo dos decimales en el monto.
        if (montoTotal is decimal monto && decimal.Round(monto, 2) != monto)
            yield return new ValidationResult(
                "El monto admite como máximo 2 decimales.",
                new[] { nameof(montoTotal) });
    }
}

public class solicitudMeta
{
    [Display(Name = "N.º de solicitud")]
    public object? id_solicitud { get; set; }

    [Display(Name = "Fecha de creación")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [DataType(DataType.Date)]
    public object? fechaCreacion { get; set; }

    [Display(Name = "Hora de creación")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [DataType(DataType.Time)]
    public object? horaCreacion { get; set; }

    [Display(Name = "Descripción")]
    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public object? descripcion { get; set; }

    [Display(Name = "Monto total")]
    [Required(ErrorMessage = Reglas.Obligatorio)]
    [Range(typeof(decimal), "0.01", "99999999.99",
           ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true,
           ErrorMessage = "El monto debe estar entre 0.01 y 99,999,999.99.")]
    [RegularExpression(Reglas.Dinero, ErrorMessage = "Escriba un monto válido, solo números y hasta 2 decimales con punto (ej. 15000.50).")]
    public object? montoTotal { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_estado { get; set; }

    [Display(Name = "Método de pago")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_metodoPago { get; set; }

    [Display(Name = "Cliente")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_cliente { get; set; }

    [Display(Name = "Domiciliario")]
    public object? id_domiciliario { get; set; }

    [Display(Name = "Fecha de asignación")]
    [DataType(DataType.Date)]
    public object? fechaAsignacion { get; set; }

    [Display(Name = "Hora de asignación")]
    [DataType(DataType.Time)]
    public object? horaAsignacion { get; set; }

    [Display(Name = "Dirección de entrega")]
    [Required(ErrorMessage = Reglas.Elegir)]
    public object? id_direccionEntrega { get; set; }

    [Display(Name = "Establecimiento de recogida")]
    public object? id_establecimientoRecogida { get; set; }

    [Display(Name = "Dirección de recogida externa")]
    public object? id_direccionRecogidaExterna { get; set; }
}
