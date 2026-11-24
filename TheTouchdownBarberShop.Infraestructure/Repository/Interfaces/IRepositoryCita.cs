using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCita
    {
        Task<int?> ObtenerSucursalEncargadoAsync(int idEncargado);
        Task<ICollection<Citas>> ListAsync(int idEncargado);
        Task<Citas?> FindByIdAsync(int id, int idEncargado);
        Task<ICollection<Usuario>> ListEncargadosAsync();

        Task AddAsync(Citas cita);
        Task UpdateAsync(Citas cita);
        Task DeleteAsync(int id);

        Task<Dictionary<int, decimal>> ObtenerPreciosServiciosAsync(ICollection<int> idsServicios);
        Task<ICollection<Usuario>> ListUsuariosPorRolAsync(string rol);
        Task<ICollection<Paquetes>> ListPaquetesAsync();
        Task<ICollection<Servicios>> ListServiciosAsync();
        Task<string?> ObtenerNombreSucursalEncargadoAsync(int idEncargado);
    }
}