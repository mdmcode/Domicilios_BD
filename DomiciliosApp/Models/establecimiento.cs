using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class establecimiento
{
    public int id_establecimiento { get; set; }

    public int? codigo { get; set; }

    public string? nombre { get; set; }

    public int? id_tipoEstablecimiento { get; set; }

    public int id_direccion { get; set; }

    public string? telefono { get; set; }

    public string? email { get; set; }

    public virtual direccion id_direccionNavigation { get; set; } = null!;

    public virtual tipoestablecimiento? id_tipoEstablecimientoNavigation { get; set; }

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
