using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryFactura : IRepositoryFactura
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryFactura(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Facturas>> ListAsync()
        {
            return await _context.Facturas
                .AsNoTracking()
                .Include(f => f.IdClienteNavigation)        
                .Include(f => f.IdMetodoPagoNavigation)     
                .OrderByDescending(f => f.FechaEmision)     
                .ToListAsync();
        }

        public async Task<Facturas?> FindByIdAsync(int id)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Include(f => f.IdClienteNavigation)
                .Include(f => f.IdMetodoPagoNavigation)
                .Include(f => f.FacturaProductos)
                    .ThenInclude(fp => fp.IdProductoNavigation)
                .FirstOrDefaultAsync(f => f.IdFactura == id);
        }
    }
}