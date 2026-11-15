using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryTarea : IRepositoryTarea
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryTarea(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracionTarea?> ObtenerConfiguracionPorNombreAsync(string nombreTarea)
        {
            return await _context.ConfiguracionTareas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.NombreTarea == nombreTarea);
        }

        public async Task<bool> ExisteEjecucionHoyAsync(string nombreTarea, DateOnly fechaHoy)
        {
            return await _context.BitacoraTareas
                .AnyAsync(b => b.NombreTarea == nombreTarea && DateOnly.FromDateTime(b.FechaEjecucion) == fechaHoy);
        }

        public async Task RegistrarBitacoraAsync(BitacoraTarea bitacora)
        {
            await _context.BitacoraTareas.AddAsync(bitacora);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BitacoraTarea>> ListarBitacoraAsync()
        {
            return await _context.BitacoraTareas
                .Include(b => b.IdProductoNavigation)
                .AsNoTracking()
                .OrderByDescending(b => b.FechaEjecucion)
                .ToListAsync();
        }
    }
}