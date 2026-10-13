using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Paquetes
{
    public int IdPaquete { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? PrecioTotal { get; set; }

    public bool EsPersonalizable { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Citas> Citas { get; set; } = new List<Citas>();

    public virtual ICollection<PaqueteServicios> PaqueteServicios { get; set; } = new List<PaqueteServicios>();
}
