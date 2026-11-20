using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class tipoincidente
{
    public int id_tipoIncidente { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<incidente> incidentes { get; set; } = new List<incidente>();
}
