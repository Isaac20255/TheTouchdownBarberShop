using System.ComponentModel.DataAnnotations;
using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.DTO
{
    public class FacturaDTO : IValidatableObject
    {
        public int IdFactura { get; set; }
        public int? IdCita { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
        public int IdCliente { get; set; }

        public int IdSucursal { get; set; }
        public int? IdEncargado { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public int? IdMetodoPago { get; set; }

        // Porcentaje de impuesto aplicado (lo asigna el servicio; la vista lo usa para el cálculo en tiempo real)
        public decimal PorcentajeImpuesto { get; set; }

        // Datos de solo lectura
        public string? Estado { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteCorreo { get; set; }
        public string? SucursalNombre { get; set; }
        public string? EncargadoNombre { get; set; }
        public string? MetodoPagoNombre { get; set; }
        public string? EstadoCita { get; set; }

        public List<FacturaProductoDTO> FacturaProductos { get; set; } = new();
        public List<FacturaServicioDTO> FacturaServicios { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hayLineas = (FacturaProductos?.Any(p => p.Cantidad > 0) ?? false)
                         || (FacturaServicios?.Any(s => s.Cantidad > 0) ?? false);

            if (!hayLineas)
            {
                yield return new ValidationResult(
                    "Debe agregar al menos un producto o servicio con cantidad mayor a 0.",
                    new[] { nameof(FacturaProductos) });
            }
        }
    }
}