using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Citas
{
    public int IdCita { get; set; }

    public int IdCliente { get; set; }

    public int? IdBarbero { get; set; }

    public int? IdPaquete { get; set; }

    public int IdSucursal { get; set; }

    public DateTime FechaHora { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<CitaPreguntas> CitaPreguntas { get; set; } = new List<CitaPreguntas>();

    public virtual ICollection<CitaServicios> CitaServicios { get; set; } = new List<CitaServicios>();

    public virtual Facturas? Facturas { get; set; }

    public virtual ICollection<HistorialCitas> HistorialCitas { get; set; } = new List<HistorialCitas>();

    public virtual Usuarios? IdBarberoNavigation { get; set; }

    public virtual Usuarios IdClienteNavigation { get; set; } = null!;

    public virtual Paquetes? IdPaqueteNavigation { get; set; }

    public virtual Sucursales IdSucursalNavigation { get; set; } = null!;

    public virtual Resenas? Resenas { get; set; }
}
