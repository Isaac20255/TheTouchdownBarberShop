using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryProducto(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            return await _context.Set<Producto>()
                .AsNoTracking()
                .Include(p => p.IdCategoriaProductoNavigation)
                .Where(p => p.Activo)
                .ToListAsync();
        }

        public async Task<Producto?> FindByIdAsync(int id)
        {
            return await _context.Set<Producto>()
                .AsNoTracking()
                .Include(p => p.IdCategoriaProductoNavigation)
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Activo);
        }

        public async Task<ICollection<CategoriasProductos>> ListCategoriasAsync()
        {
            return await _context.Set<CategoriasProductos>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = false;
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
    }
}
}



