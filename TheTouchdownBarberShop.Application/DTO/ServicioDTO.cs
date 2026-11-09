namespace TheTouchdownBarberShop.Application.DTOs
{
    public class ServicioDTO
    {
        public int IdServicio { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; }
        public string? NombreCategoria { get; set; }
        public string? Imagen { get; set; }
    }
}