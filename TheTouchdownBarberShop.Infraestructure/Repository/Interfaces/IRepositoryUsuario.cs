using TheTouchdownBarberShop.Infraestructure.Models;

public interface IRepositoryUsuario
{
    Task<ICollection<Usuario>> ListAsync();
    Task<Usuario?> FindByIdAsync(int id);
}