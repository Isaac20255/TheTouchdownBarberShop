using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCita
    {
        Task<ICollection<Citas>> ListAsync(int idEncargado);
        Task<Citas?> FindByIdAsync(int id, int idEncargado);
        Task<ICollection<Usuario>> ListEncargadosAsync();
    }
}