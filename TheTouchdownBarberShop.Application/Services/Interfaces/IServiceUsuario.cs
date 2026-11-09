using TheTouchdownBarberShop.Application.DTOs;

public interface IServiceUsuario
{
    Task<ICollection<UsuarioDTO>> ListAsync();
    Task<UsuarioDTO?> FindByIdAsync(int id);
}