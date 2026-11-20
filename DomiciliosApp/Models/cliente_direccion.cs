using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class cliente_direccion
{
    public int id_cliente { get; set; }

    public int id_direccion { get; set; }

    public virtual cliente id_clienteNavigation { get; set; } = null!;

    public virtual direccion id_direccionNavigation { get; set; } = null!;

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
