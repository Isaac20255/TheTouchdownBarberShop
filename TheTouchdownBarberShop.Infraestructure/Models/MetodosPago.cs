using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class MetodosPago
{
    public int IdMetodoPago { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();
}
