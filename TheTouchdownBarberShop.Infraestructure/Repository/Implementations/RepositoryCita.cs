using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryCita : IRepositoryCita
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryCita(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Citas>> ListAsync(int idEncargado)
        {
            return await _context.Set<Citas>()
                .AsNoTracking()
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdBarberoNavigation)
                .Include(c => c.IdSucursalNavigation)
                .Where(c => c.IdSucursal == _context.Set<Usuario>()
                    .Where(u => u.IdUsuario == idEncargado
                             && u.IdRolNavigation.Nombre == "Encargado")
                    .Select(u => u.IdSucursal)
                    .FirstOrDefault())
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        public async Task<Citas?> FindByIdAsync(int id, int idEncargado)
        {
            return await _context.Set<Citas>()
                .AsNoTracking()
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdBarberoNavigation)
                .Include(c => c.IdSucursalNavigation)
                .Include(c => c.IdPaqueteNavigation)
                .Include(c => c.CitaServicios)
                    .ThenInclude(cs => cs.IdServicioNavigation)
                .Include(c => c.CitaPreguntas)
                .Where(c => c.IdCita == id
                         && c.IdSucursal == _context.Set<Usuario>()
                            .Where(u => u.IdUsuario == idEncargado
                                     && u.IdRolNavigation.Nombre == "Encargado")
                            .Select(u => u.IdSucursal)
                            .FirstOrDefault())
                .FirstOrDefaultAsync();
        }

        // Encargados registrados en la BD con su sucursal (para llenar el combo)
        public async Task<ICollection<Usuario>> ListEncargadosAsync()
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Include(u => u.IdSucursalNavigation)
                .Where(u => u.IdRolNavigation.Nombre == "Encargado" && u.IdSucursal != null)
                .OrderBy(u => u.IdSucursalNavigation!.Nombre)
                .ToListAsync();
        }
    }
}