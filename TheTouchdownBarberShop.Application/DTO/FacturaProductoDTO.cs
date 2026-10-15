namespace TheTouchdownBarberShop.Application.DTOs
{
    public class FacturaProductoDTO
    {
        public int IdProducto { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }                // supuesto
        public decimal PrecioUnitario { get; set; }      // supuesto
        public decimal Subtotal { get; set; }            // supuesto
    }
}