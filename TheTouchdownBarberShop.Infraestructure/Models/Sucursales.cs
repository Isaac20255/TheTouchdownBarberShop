using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Sucursales
{
    public int IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public virtual ICollection<Citas> Citas { get; set; } = new List<Citas>();

    public virtual ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
