namespace TheTouchdownBarberShop.Infraestructure.Models
{
    public partial class BitacoraTarea
    {
        public int IdBitacora { get; set; }
        public string NombreTarea { get; set; } = null!;
        public DateTime FechaEjecucion { get; set; }
        public int? IdProducto { get; set; }
        public bool CorreoEnviado { get; set; }
        public string Resultado { get; set; } = null!;

        public virtual Producto? IdProductoNavigation { get; set; }
    }
}