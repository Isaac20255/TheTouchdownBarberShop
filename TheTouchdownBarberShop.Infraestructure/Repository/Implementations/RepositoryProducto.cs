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

        //Tarea Programada

        public async Task<Producto?> ObtenerProductoDelDiaAsync(DateOnly fecha)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.EsProductoDelDia && p.FechaProductoDelDia == fecha);
        }

        public async Task MarcarProductoDelDiaAsync(int idProducto, DateOnly fecha)
        {
            var anteriores = await _context.Productos.Where(p => p.EsProductoDelDia).ToListAsync();
            foreach (var p in anteriores)
            {
                p.EsProductoDelDia = false;
                p.FechaProductoDelDia = null;
            }

            var nuevo = await _context.Productos.FindAsync(idProducto);
            if (nuevo != null)
            {
                nuevo.EsProductoDelDia = true;
                nuevo.FechaProductoDelDia = fecha;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Producto>> ListarActivosAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo == true) // Cambia 'Activo' por el nombre real de tu propiedad booleana de activo/inactivo si se llama diferente (ej: Activo)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}



