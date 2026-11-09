namespace TheTouchdownBarberShop.Application.DTOs
{
    public record HorarioSucursalDTO
    {
        public int IdHorarioSucursal { get; set; }
        public int IdSucursal { get; set; }
        public string? SucursalNombre { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public List<string> DiasSeleccionados { get; set; } = new();
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}