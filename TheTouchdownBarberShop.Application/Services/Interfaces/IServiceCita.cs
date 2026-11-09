using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceCita
    {
        Task<ICollection<CitaDTO>> ListAsync(int idEncargado);
        Task<CitaDTO?> FindByIdAsync(int id, int idEncargado);
        Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync();
    }
}