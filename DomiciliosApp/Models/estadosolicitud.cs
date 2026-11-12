using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class estadosolicitud
{
    public int id_estado { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
