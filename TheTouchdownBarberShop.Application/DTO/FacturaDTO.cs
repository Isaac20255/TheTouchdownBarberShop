using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.DTO
{
    public class FacturaDTO
    {
        public int IdFactura { get; set; }
        public int? IdCita { get; set; }
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public int IdMetodoPago { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public string MetodoPagoNombre { get; set; } = string.Empty;

        public ICollection<FacturaProductoDTO> FacturaProductos { get; set; } = new List<FacturaProductoDTO>();

    }
}