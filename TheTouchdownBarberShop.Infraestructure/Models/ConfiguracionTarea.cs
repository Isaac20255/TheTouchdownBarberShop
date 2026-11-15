namespace TheTouchdownBarberShop.Infraestructure.Models
{
    public partial class ConfiguracionTarea
    {
        public int IdConfiguracion { get; set; }
        public string NombreTarea { get; set; } = null!;
        public bool Activa { get; set; }
        public TimeOnly HoraEjecucion { get; set; }
        public string CorreoDestino { get; set; } = null!;
    }
}