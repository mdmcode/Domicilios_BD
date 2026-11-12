using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class mediodetransporte
{
    public int id_transporte { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<domiciliario> domiciliarios { get; set; } = new List<domiciliario>();
}
