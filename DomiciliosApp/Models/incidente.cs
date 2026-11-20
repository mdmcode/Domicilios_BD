using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class incidente
{
    public int id_incidente { get; set; }

    public int tipoIncidente { get; set; }

    public string? descripcion { get; set; }

    public DateOnly fecha { get; set; }

    public TimeOnly hora { get; set; }

    public int id_solicitud { get; set; }

    public virtual solicitud id_solicitudNavigation { get; set; } = null!;

    public virtual tipoincidente tipoIncidenteNavigation { get; set; } = null!;
}
