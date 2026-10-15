using TheTouchdownBarberShop.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTouchdownBarberShop.Application.DTOs
{
    public record ProductoDTO
    {
        public int IdProducto { get; set; }
        public int IdCategoriaProducto { get; set; }
        public string CategoriaNombre { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }
        public string? Marca { get; set; }
        public string? Presentacion { get; set; }
        public bool Activo { get; set; }
    }
}