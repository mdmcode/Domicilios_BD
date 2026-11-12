using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class estadodomiciliario
{
    public int id_estado { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<domiciliario> domiciliarios { get; set; } = new List<domiciliario>();
}
