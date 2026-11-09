using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryServicio
    {
        Task<ICollection<Servicios>> ListAsync();
        Task<Servicios?> FindByIdAsync(int id);
        Task<ICollection<CategoriasServicios>> ListCategoriasAsync();
        Task AddAsync(Servicios servicio);
        Task UpdateAsync(Servicios servicio);
        Task DeleteAsync(int id);
    }
}