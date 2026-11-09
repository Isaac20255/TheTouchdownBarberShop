namespace TheTouchdownBarberShop.Application.DTOs
{
    public class CitaDTO
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
    }
}