using System;
using System.Collections.Generic;

namespace DomiciliosApp.Models;

public partial class usuario
{
    public int id_usuario { get; set; }

    public string numeroId { get; set; } = null!;

    public string nombre_completo { get; set; } = null!;

    public string telefono { get; set; } = null!;

    public string correo { get; set; } = null!;

    public virtual cliente? cliente { get; set; }

    public virtual domiciliario? domiciliario { get; set; }
}
