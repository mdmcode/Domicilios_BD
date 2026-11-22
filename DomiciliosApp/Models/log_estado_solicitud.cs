namespace DomiciliosApp.Models;

public partial class log_estado_solicitud
{
    public int id { get; set; }
    public int? id_solicitud { get; set; }
    public int? estado_anterior { get; set; }
    public int? estado_nuevo { get; set; }
    public DateOnly? fecha_cambio { get; set; }
}