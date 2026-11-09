using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositorySucursal : IRepositorySucursal
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositorySucursal(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Sucursal>> ListAsync()
        {
            return await _context.Sucursales
                .AsNoTracking()
                .Include(s => s.Administrador)
                .OrderBy(s => s.IdSucursal)
                .ToListAsync();
        }

        public async Task<Sucursal?> FindByIdAsync(int id)
        {
            return await _context.Sucursales
                .AsNoTracking()
                .Include(s => s.Administrador)
                    .ThenInclude(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(s => s.IdSucursal == id);
        }

        public async Task AddAsync(Sucursal sucursal)
        {
            await _context.Sucursales.AddAsync(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sucursal sucursal)
        {
            _context.Sucursales.Update(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal != null)
            {
                _context.Sucursales.Remove(sucursal);
                await _context.SaveChangesAsync();
            }
        }
    }
}