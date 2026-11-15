using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoriaProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? Imagen { get; set; }

    public string? Marca { get; set; }

    public string? Presentacion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<FacturaProductos> FacturaProductos { get; set; } = new List<FacturaProductos>();

    public virtual CategoriasProductos IdCategoriaProductoNavigation { get; set; } = null!;

    [Column("es_producto_del_dia")]
    public bool EsProductoDelDia { get; set; }

    [Column("fecha_producto_del_dia")]
    public DateOnly? FechaProductoDelDia { get; set; }
}