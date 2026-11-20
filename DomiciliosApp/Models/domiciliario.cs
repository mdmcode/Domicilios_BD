using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class domiciliario
{
    public int id_domiciliario { get; set; }

    public int? codigo { get; set; }

    public int id_transporte { get; set; }

    public int id_estado { get; set; }

    public virtual usuario id_domiciliarioNavigation { get; set; } = null!;

    public virtual estadodomiciliario id_estadoNavigation { get; set; } = null!;

    public virtual mediodetransporte id_transporteNavigation { get; set; } = null!;

    public virtual ICollection<solicitud> solicituds { get; set; } = new List<solicitud>();
}
