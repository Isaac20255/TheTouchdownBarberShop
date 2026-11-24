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

        public async Task<string?> ObtenerNombreSucursalEncargadoAsync(int idEncargado)
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Where(u => u.IdUsuario == idEncargado
                         && u.IdRolNavigation != null
                         && u.IdRolNavigation.Nombre == "Encargado"
                         && u.IdSucursalNavigation != null)
                .Select(u => u.IdSucursalNavigation!.Nombre)
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

        public async Task AddAsync(Citas cita)
        {
            await _context.Set<Citas>().AddAsync(cita);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Citas cita)
        {
            var existente = await _context.Set<Citas>()
                .Include(c => c.CitaServicios)
                .Include(c => c.CitaPreguntas)
                .Include(c => c.Facturas)
                    .ThenInclude(f => f!.FacturaServicios)
                .FirstOrDefaultAsync(c => c.IdCita == cita.IdCita);

            if (existente == null)
            {
                return;
            }

            // Copia solo las propiedades escalares; las navegaciones no se tocan.
            _context.Entry(existente).CurrentValues.SetValues(cita);

            // Reemplazo de las colecciones hijas. La FK la asigna EF por la navegación.
            _context.RemoveRange(existente.CitaServicios);
            _context.RemoveRange(existente.CitaPreguntas);

            foreach (var servicio in cita.CitaServicios)
            {
                existente.CitaServicios.Add(servicio);
            }
            foreach (var pregunta in cita.CitaPreguntas)
            {
                existente.CitaPreguntas.Add(pregunta);
            }

            // Proforma: se actualiza si existe y sigue en estado Proforma, o se crea si no existía y la cita no está completada.
            if (cita.Facturas != null && existente.Estado != "Completada")
            {
                if (existente.Facturas != null && existente.Facturas.Estado == "Proforma")
                {
                    var proforma = existente.Facturas;

                    proforma.IdCliente = cita.IdCliente;
                    proforma.IdSucursal = cita.IdSucursal;
                    proforma.Subtotal = cita.Facturas.Subtotal;
                    proforma.Impuesto = cita.Facturas.Impuesto;
                    proforma.Total = cita.Facturas.Total;

                    _context.RemoveRange(proforma.FacturaServicios);
                    foreach (var linea in cita.Facturas.FacturaServicios)
                    {
                        proforma.FacturaServicios.Add(linea);
                    }
                }
                else if (existente.Facturas == null)
                {
                    cita.Facturas.IdCita = existente.IdCita;
                    existente.Facturas = cita.Facturas;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _context.Set<Citas>()
                .Include(c => c.CitaServicios)
                .Include(c => c.CitaPreguntas)
                .Include(c => c.HistorialCitas)
                .Include(c => c.Resenas)
                .Include(c => c.Facturas)
                    .ThenInclude(f => f!.FacturaServicios)
                .Include(c => c.Facturas)
                    .ThenInclude(f => f!.FacturaProductos)
                .FirstOrDefaultAsync(c => c.IdCita == id);

            if (cita == null)
            {
                return;
            }

            // Una cita completada o con factura ya registrada no se elimina.
            if (cita.Estado == "Completada" ||
                (cita.Facturas != null && cita.Facturas.Estado == "Facturado"))
            {
                throw new InvalidOperationException(
                    "No se puede eliminar la cita porque está completada o tiene una factura registrada.");
            }

            // La proforma (si existe) se elimina junto con sus líneas.
            if (cita.Facturas != null)
            {
                _context.RemoveRange(cita.Facturas.FacturaServicios);
                _context.RemoveRange(cita.Facturas.FacturaProductos);
                _context.Remove(cita.Facturas);
            }

            _context.RemoveRange(cita.CitaServicios);
            _context.RemoveRange(cita.CitaPreguntas);
            _context.RemoveRange(cita.HistorialCitas);
            if (cita.Resenas != null)
            {
                _context.Remove(cita.Resenas);
            }

            _context.Set<Citas>().Remove(cita);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, decimal>> ObtenerPreciosServiciosAsync(ICollection<int> idsServicios)
        {
            return await _context.Set<Servicios>()
                .AsNoTracking()
                .Where(s => idsServicios.Contains(s.IdServicio))
                .ToDictionaryAsync(s => s.IdServicio, s => s.Precio);
        }

        public async Task<ICollection<Usuario>> ListUsuariosPorRolAsync(string rol)
        {
            return await _context.Set<Usuario>()
                .AsNoTracking()
                .Where(u => u.IdRolNavigation != null && u.IdRolNavigation.Nombre == rol)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Paquetes>> ListPaquetesAsync()
        {
            return await _context.Set<Paquetes>()
                .AsNoTracking()
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
    }
}