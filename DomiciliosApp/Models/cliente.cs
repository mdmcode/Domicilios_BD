using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class cliente
{
    public int id_cliente { get; set; }

    public virtual ICollection<cliente_direccion> cliente_direccions { get; set; } = new List<cliente_direccion>();

    public virtual usuario id_clienteNavigation { get; set; } = null!;

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
