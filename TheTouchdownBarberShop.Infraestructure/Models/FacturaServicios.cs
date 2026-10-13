using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class FacturaServicios
{
    public int IdFacturaServicio { get; set; }

    public int IdFactura { get; set; }

    public int IdServicio { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Facturas IdFacturaNavigation { get; set; } = null!;

    public virtual Servicios IdServicioNavigation { get; set; } = null!;
}
