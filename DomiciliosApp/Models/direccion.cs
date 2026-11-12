using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class direccion
{
    public int id_direccion { get; set; }

    public string? ubicacion { get; set; }

    public string? barrio { get; set; }

    public string? comuna { get; set; }

    public string? referencia { get; set; }

    public virtual ICollection<cliente_direccion> cliente_direccions { get; set; } = new List<cliente_direccion>();

    public virtual ICollection<establecimiento> establecimientos { get; set; } = new List<establecimiento>();

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
