using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Facturas
{
    public int IdFactura { get; set; }

    public int? IdCita { get; set; }

    public int IdCliente { get; set; }

    public int IdSucursal { get; set; }

    public DateTime FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public int IdMetodoPago { get; set; }

    public virtual ICollection<FacturaProductos> FacturaProductos { get; set; } = new List<FacturaProductos>();

    public virtual ICollection<FacturaServicios> FacturaServicios { get; set; } = new List<FacturaServicios>();

    public virtual Citas? IdCitaNavigation { get; set; }

    public virtual Usuarios IdClienteNavigation { get; set; } = null!;

    public virtual MetodosPago IdMetodoPagoNavigation { get; set; } = null!;

    public virtual Sucursales IdSucursalNavigation { get; set; } = null!;
}
