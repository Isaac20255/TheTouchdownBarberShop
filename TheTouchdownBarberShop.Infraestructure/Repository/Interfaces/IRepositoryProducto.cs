
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<ICollection<Producto>> ListAsync();
        Task<Producto?> FindByIdAsync(int id);
        Task<ICollection<CategoriasProductos>> ListCategoriasAsync();
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(int id);


        //Tarea programada

        Task<Producto?> ObtenerProductoDelDiaAsync(DateOnly fecha);
        Task MarcarProductoDelDiaAsync(int idProducto, DateOnly fecha);
        Task<IEnumerable<Producto>> ListarActivosAsync();

    }
}
