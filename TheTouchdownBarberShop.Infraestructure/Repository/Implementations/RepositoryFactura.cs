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

        // ---------- Existentes ----------

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

        // ---------- Encargado ----------

        public async Task<int?> ObtenerSucursalEncargadoAsync(int idEncargado)
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

        public async Task<Usuario?> FindEncargadoAsync(int idEncargado)
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Include(u => u.IdSucursalNavigation)
                .Where(u => u.IdUsuario == idEncargado
                         && u.IdRolNavigation != null
                         && u.IdRolNavigation.Nombre == "Encargado"
                         && u.IdSucursal != null
                         && u.IdSucursalNavigation != null)
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

        // ---------- Historial y detalle por encargado ----------

        public async Task<ICollection<Facturas>> ListByEncargadoAsync(int idEncargado)
        {
            var idSucursal = await ObtenerSucursalEncargadoAsync(idEncargado);
            if (!idSucursal.HasValue)
            {
                return new List<Facturas>();
            }

            return await _context.Set<Facturas>()
                .AsNoTracking()
                .Include(f => f.IdClienteNavigation)
                .Include(f => f.IdSucursalNavigation)
                .Include(f => f.IdMetodoPagoNavigation)
                .Include(f => f.IdCitaNavigation)
                .Include(f => f.FacturaServicios)
                    .ThenInclude(fs => fs.IdServicioNavigation)
                .Where(f => f.IdSucursal == idSucursal.Value)
                .OrderByDescending(f => f.FechaEmision)
                .ToListAsync();
        }

        public async Task<Facturas?> FindByIdAsync(int id, int idEncargado)
        {
            var idSucursal = await ObtenerSucursalEncargadoAsync(idEncargado);
            if (!idSucursal.HasValue)
            {
                return null;
            }

            return await _context.Set<Facturas>()
                .AsNoTracking()
                .Include(f => f.IdClienteNavigation)
                .Include(f => f.IdSucursalNavigation)
                .Include(f => f.IdEncargadoNavigation)
                .Include(f => f.IdMetodoPagoNavigation)
                .Include(f => f.IdCitaNavigation)
                .Include(f => f.FacturaProductos)
                    .ThenInclude(fp => fp.IdProductoNavigation)
                .Include(f => f.FacturaServicios)
                    .ThenInclude(fs => fs.IdServicioNavigation)
                .Where(f => f.IdFactura == id && f.IdSucursal == idSucursal.Value)
                .FirstOrDefaultAsync();
        }

        // ---------- Catálogos del formulario ----------

        public async Task<ICollection<Usuario>> ListClientesAsync()
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Where(u => u.IdRolNavigation != null && u.IdRolNavigation.Nombre == "Cliente")
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Producto>> ListProductosAsync()
        {
            return await _context.Set<Producto>()
                .AsNoTracking()
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Servicios>> ListServiciosAsync()
        {
            return await _context.Set<Servicios>()
                .AsNoTracking()
                .Where(s => s.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();
        }

        // ---------- Registro ----------

        public async Task AddAsync(Facturas factura)
        {
            await _context.Set<Facturas>().AddAsync(factura);
            await _context.SaveChangesAsync();
        }

        // Convierte una proforma en factura reemplazando sus líneas de detalle.
        // Devuelve false si no existe o si ya no es una proforma.
        public async Task<bool> UpdateAsync(Facturas factura)
        {
            var existente = await _context.Set<Facturas>()
                .Include(f => f.FacturaProductos)
                .Include(f => f.FacturaServicios)
                .FirstOrDefaultAsync(f => f.IdFactura == factura.IdFactura);

            if (existente == null || existente.Estado != "Proforma")
            {
                return false;
            }

            // Copia las propiedades escalares; las navegaciones no se tocan.
            _context.Entry(existente).CurrentValues.SetValues(factura);

            _context.RemoveRange(existente.FacturaProductos);
            _context.RemoveRange(existente.FacturaServicios);

            foreach (var producto in factura.FacturaProductos)
            {
                existente.FacturaProductos.Add(producto);
            }
            foreach (var servicio in factura.FacturaServicios)
            {
                existente.FacturaServicios.Add(servicio);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}