using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class metodopago
{
    public int id_metodoPago { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
