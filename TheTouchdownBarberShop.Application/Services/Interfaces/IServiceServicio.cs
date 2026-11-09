using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceServicio
    {
        Task<ICollection<ServicioDTO>> ListAsync();
        Task<ServicioDTO?> FindByIdAsync(int id);
        Task<ICollection<CategoriasServicios>> ListCategoriasAsync();
        Task AddAsync(ServicioDTO servicioDto);
        Task UpdateAsync(ServicioDTO servicioDto);
        Task DeleteAsync(int id);
    }
}