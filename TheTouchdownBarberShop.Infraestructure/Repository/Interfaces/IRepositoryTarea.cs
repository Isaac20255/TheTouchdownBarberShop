using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryTarea
    {
        Task<ConfiguracionTarea?> ObtenerConfiguracionPorNombreAsync(string nombreTarea);
        Task<bool> ExisteEjecucionHoyAsync(string nombreTarea, DateOnly fechaHoy);
        Task RegistrarBitacoraAsync(BitacoraTarea bitacora);
        Task<ICollection<BitacoraTarea>> ListarBitacoraAsync();
    }
}