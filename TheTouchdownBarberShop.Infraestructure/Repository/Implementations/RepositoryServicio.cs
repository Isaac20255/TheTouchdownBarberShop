using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryServicio : IRepositoryServicio
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryServicio(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Servicios>> ListAsync()
        {
            return await _context.Set<Servicios>()
                .AsNoTracking()
                .Include(s => s.IdCategoriaNavigation)
                .ToListAsync();
        }

        public async Task<Servicios?> FindByIdAsync(int id)
        {
            return await _context.Set<Servicios>()
                .AsNoTracking()
                .Include(s => s.IdCategoriaNavigation)
                .FirstOrDefaultAsync(s => s.IdServicio == id);
        }

        public async Task<ICollection<CategoriasServicios>> ListCategoriasAsync()
        {
            return await _context.Set<CategoriasServicios>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Servicios servicio)
        {
            await _context.Set<Servicios>().AddAsync(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Servicios servicio)
        {
            _context.Set<Servicios>().Update(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var servicio = await _context.Set<Servicios>().FindAsync(id);
            if (servicio != null)
            {
                _context.Set<Servicios>().Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }
    }
}