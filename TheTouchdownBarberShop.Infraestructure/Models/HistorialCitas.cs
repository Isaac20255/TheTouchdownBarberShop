using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class HistorialCitas
{
    public int IdHistorial { get; set; }

    public int IdCita { get; set; }

    public string? EstadoAnterior { get; set; }

    public string EstadoNuevo { get; set; } = null!;

    public DateTime FechaCambio { get; set; }

    public int IdUsuarioCambio { get; set; }

    public virtual Citas IdCitaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioCambioNavigation { get; set; } = null!;
}
