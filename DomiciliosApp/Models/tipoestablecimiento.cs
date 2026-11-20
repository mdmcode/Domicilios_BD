using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class tipoestablecimiento
{
    public int id_tipoEstablecimiento { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<establecimiento> establecimientos { get; set; } = new List<establecimiento>();
}
