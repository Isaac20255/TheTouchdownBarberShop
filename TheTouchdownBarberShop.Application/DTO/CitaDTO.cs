using System.ComponentModel.DataAnnotations;

namespace TheTouchdownBarberShop.Application.DTOs
{
    public class CitaDTO : IValidatableObject
    {
        public int IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string NombreCliente { get; set; } = "";
        public string NombreBarbero { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Sucursal { get; set; } = "";

        // Solo se usan en el detalle
        public string? Paquete { get; set; }
        public string? Observaciones { get; set; }
        public decimal Total { get; set; }
        public List<string> Servicios { get; set; } = new();
        public List<string> Preguntas { get; set; } = new();

        // Solo se usan en crear y editar
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
        public int IdCliente { get; set; }
        public int? IdBarbero { get; set; }
        public int? IdPaquete { get; set; }
        public List<int> IdsServicios { get; set; } = new();
        public Dictionary<string, string?> Respuestas { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IdCita == 0 && FechaHora.Date < DateTime.Today)
            {
                yield return new ValidationResult(
                    "La fecha no puede ser anterior a hoy.", new[] { nameof(FechaHora) });
            }

            if (IdsServicios == null || IdsServicios.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe seleccionar al menos un servicio.", new[] { nameof(IdsServicios) });
            }

            if (Respuestas == null || Respuestas.Count(r => !string.IsNullOrWhiteSpace(r.Value)) < 3)
            {
                yield return new ValidationResult(
                    "Debe responder como mínimo 3 preguntas.", new[] { nameof(Respuestas) });
            }
        }
    }
}