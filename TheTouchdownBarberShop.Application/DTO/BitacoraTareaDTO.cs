namespace TheTouchdownBarberShop.Application.DTOs
{
    public class BitacoraTareaDTO
    {
        public int IdBitacora { get; set; }
        public string NombreTarea { get; set; } = null!;
        public DateTime FechaEjecucion { get; set; }
        public int? IdProducto { get; set; }
        public bool CorreoEnviado { get; set; }
        public string Resultado { get; set; } = null!;

        // Propiedad opcional para mostrar el nombre del producto directamente en la vista si lo deseas
        public ProductoDTO? ProductoNavigation { get; set; }
    }
}