using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceProducto
    {
        Task<ICollection<ProductoDTO>> ListAsync();

        Task<ProductoDTO?> FindByIdAsync(int id);

        Task<ICollection<CategoriasProductos>> ListCategoriasAsync();

        Task AddAsync(ProductoDTO productoDto);                 
        Task UpdateAsync(ProductoDTO productoDto);

        Task DeleteAsync(int id);
    }
}