using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class CitaServicios
{
    public int IdCitaServicio { get; set; }

    public int IdCita { get; set; }

    public int IdServicio { get; set; }

    public decimal PrecioAplicado { get; set; }

    public virtual Citas IdCitaNavigation { get; set; } = null!;

    public virtual Servicios IdServicioNavigation { get; set; } = null!;
}
