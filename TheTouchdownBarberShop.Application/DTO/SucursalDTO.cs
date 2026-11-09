namespace TheTouchdownBarberShop.Application.DTO
{
    public class SucursalDTO
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string AdministradorNombre { get; set; } = string.Empty;
        public int? AdministradorId { get; set; }

        public bool EstaAbierta { get; set; }
    }
}