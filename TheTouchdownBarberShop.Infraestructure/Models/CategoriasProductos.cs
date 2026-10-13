using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class CategoriasProductos
{
    public int IdCategoriaProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
