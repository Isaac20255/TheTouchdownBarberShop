using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceCita
    {
        Task<ICollection<CitaDTO>> ListAsync(int idEncargado, string? cliente = null, DateTime? fecha = null);
        Task<CitaDTO?> FindByIdAsync(int id, int idEncargado);
        Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync();

        Task<int?> AddAsync(CitaDTO dto, int idEncargado);
        Task<bool> UpdateAsync(CitaDTO dto, int idEncargado);
        Task<bool> DeleteAsync(int id, int idEncargado);

        Task<List<(int Id, string Descripcion)>> ListClientesAsync();
        Task<List<(int Id, string Descripcion)>> ListBarberosAsync();
        Task<List<(int Id, string Descripcion)>> ListPaquetesAsync();
        Task<List<(int Id, string Descripcion)>> ListServiciosAsync();
        Task<string?> ObtenerNombreSucursalEncargadoAsync(int idEncargado);
    }
}