using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Servicios
{
    public int IdServicio { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int DuracionMinutos { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<CitaServicios> CitaServicios { get; set; } = new List<CitaServicios>();

    public virtual ICollection<FacturaServicios> FacturaServicios { get; set; } = new List<FacturaServicios>();

    public virtual CategoriasServicios IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<PaqueteServicios> PaqueteServicios { get; set; } = new List<PaqueteServicios>();
}
