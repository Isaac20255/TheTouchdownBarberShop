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

        private async Task<int?> ObtenerSucursalEncargadoAsync(int idEncargado)
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Where(u => u.IdUsuario == idEncargado
                         && u.IdRolNavigation != null
                         && u.IdRolNavigation.Nombre == "Encargado"
                         && u.IdSucursal != null)
                .Select(u => u.IdSucursal)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Citas>> ListAsync(int idEncargado)
        {
            var idSucursal = await ObtenerSucursalEncargadoAsync(idEncargado);
            if (!idSucursal.HasValue)
            {
                return new List<Citas>();
            }

            return await _context.Set<Citas>()
                .AsNoTracking()
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdBarberoNavigation)
                .Include(c => c.IdSucursalNavigation)
                .Where(c => c.IdSucursal == idSucursal.Value)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }

        public async Task<Citas?> FindByIdAsync(int id, int idEncargado)
        {
            var idSucursal = await ObtenerSucursalEncargadoAsync(idEncargado);
            if (!idSucursal.HasValue)
            {
                return null;
            }

            return await _context.Set<Citas>()
                .AsNoTracking()
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdBarberoNavigation)
                .Include(c => c.IdSucursalNavigation)
                .Include(c => c.IdPaqueteNavigation)
                .Include(c => c.CitaServicios)
                    .ThenInclude(cs => cs.IdServicioNavigation)
                .Include(c => c.CitaPreguntas)
                .Where(c => c.IdCita == id && c.IdSucursal == idSucursal.Value)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Usuario>> ListEncargadosAsync()
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Include(u => u.IdSucursalNavigation)
                .Where(u => u.IdRolNavigation != null
                         && u.IdRolNavigation.Nombre == "Encargado"
                         && u.IdSucursal != null
                         && u.IdSucursalNavigation != null)
                .OrderBy(u => u.IdSucursalNavigation!.Nombre)
                .ToListAsync();
        }
    }
}