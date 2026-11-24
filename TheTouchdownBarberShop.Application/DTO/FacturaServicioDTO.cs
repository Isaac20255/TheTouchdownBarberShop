using System.ComponentModel.DataAnnotations;

namespace TheTouchdownBarberShop.Application.DTOs
{
    public class FacturaServicioDTO
    {
        public int IdServicio { get; set; }
        public string? ServicioNombre { get; set; }

        [Range(0, 100000, ErrorMessage = "La cantidad debe ser un número entero válido.")]
        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }

        public decimal TotalLinea => Subtotal + Impuesto;
    }
}