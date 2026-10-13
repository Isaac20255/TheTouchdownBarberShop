using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class PaqueteServicios
{
    public int IdPaqueteServicio { get; set; }

    public int IdPaquete { get; set; }

    public int IdServicio { get; set; }

    public virtual Paquetes IdPaqueteNavigation { get; set; } = null!;

    public virtual Servicios IdServicioNavigation { get; set; } = null!;
}
