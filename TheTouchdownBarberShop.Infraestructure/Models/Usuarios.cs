using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public int? IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Citas> CitasIdBarberoNavigation { get; set; } = new List<Citas>();

    public virtual ICollection<Citas> CitasIdClienteNavigation { get; set; } = new List<Citas>();

    public virtual ICollection<Facturas> Facturas { get; set; } = new List<Facturas>();

    public virtual ICollection<HistorialCitas> HistorialCitas { get; set; } = new List<HistorialCitas>();

    public virtual ICollection<HorariosBarberos> HorariosBarberos { get; set; } = new List<HorariosBarberos>();

    public virtual Roles IdRolNavigation { get; set; } = null!;

    public virtual Sucursales? IdSucursalNavigation { get; set; }

    public virtual ICollection<Resenas> Resenas { get; set; } = new List<Resenas>();
}
