using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class solicitud
{
    public int id_solicitud { get; set; }

    public DateOnly fechaCreacion { get; set; }

    public TimeOnly horaCreacion { get; set; }

    public string? descripcion { get; set; }

    public decimal montoTotal { get; set; }

    public int id_estado { get; set; }

    public int id_metodoPago { get; set; }

    public int id_cliente { get; set; }

    public int? id_domiciliario { get; set; }

    public DateOnly? fechaAsignacion { get; set; }

    public TimeOnly? horaAsignacion { get; set; }

    public int id_direccionEntrega { get; set; }

    public int? id_establecimientoRecogida { get; set; }

    public int? id_direccionRecogidaExterna { get; set; }

    public virtual cliente_direccion cliente_direccion { get; set; } = null!;

    public virtual cliente id_clienteNavigation { get; set; } = null!;

    public virtual direccion? id_direccionRecogidaExternaNavigation { get; set; }

    public virtual domiciliario? id_domiciliarioNavigation { get; set; }

    public virtual establecimiento? id_establecimientoRecogidaNavigation { get; set; }

    public virtual estadosolicitud id_estadoNavigation { get; set; } = null!;

    public virtual metodopago id_metodoPagoNavigation { get; set; } = null!;

    public virtual ICollection<incidente> incidentes { get; set; } = new List<incidente>();
}
