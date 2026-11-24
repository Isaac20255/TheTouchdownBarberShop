using AutoMapper;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceFactura : IServiceFactura
    {
        private const decimal PorcentajeImpuesto = 0.13m;

        private readonly IRepositoryFactura _repository;
        private readonly IMapper _mapper;

        public ServiceFactura(IRepositoryFactura repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private static readonly HashSet<string> EstadosCitaProformaActiva = new(StringComparer.OrdinalIgnoreCase)
{
    "Pendiente",
    "Confirmada",
    "Cancelada"
};

        // ---------- Existentes ----------

        public async Task<FacturaDTO?> FindByIdAsync(int id)
        {
            // Obtener la entidad del repositorio (null si no existe)
            var @object = await _repository.FindByIdAsync(id);

            // Map Facturas a FacturaDTO (si es null, el resultado también es null)
            var objectMapped = _mapper.Map<FacturaDTO?>(@object);

            return objectMapped;
        }

        public async Task<ICollection<FacturaDTO>> ListAsync()
        {
            // Obtener datos del repositorio, ya ordenados por fecha descendente
            var list = await _repository.ListAsync();

            // Map ICollection<Facturas> a ICollection<FacturaDTO>
            var collection = _mapper.Map<ICollection<FacturaDTO>>(list);

            return collection;
        }

        // ---------- Historial y detalle por encargado ----------

        public async Task<ICollection<FacturaDTO>> ListAsync(int idEncargado, string? cliente = null, DateTime? fecha = null)
        {
            var list = await _repository.ListByEncargadoAsync(idEncargado);
            var mapped = _mapper.Map<List<FacturaDTO>>(list);

            // Si es Proforma, solo se muestra mientras la cita asociada esté Pendiente, Confirmada o Cancelada.
            mapped = mapped
                .Where(f =>
                    !string.Equals(f.Estado, "Proforma", StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrWhiteSpace(f.EstadoCita) && EstadosCitaProformaActiva.Contains(f.EstadoCita)))
                .ToList();

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                var termino = cliente.Trim();
                mapped = mapped
                    .Where(f => !string.IsNullOrEmpty(f.ClienteNombre) &&
                                f.ClienteNombre.Contains(termino, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fecha.HasValue)
            {
                var fechaBuscada = fecha.Value.Date;
                mapped = mapped
                    .Where(f => f.FechaEmision.Date == fechaBuscada)
                    .ToList();
            }

            foreach (var dto in mapped)
            {
                CompletarCalculos(dto);
            }

            return mapped;
        }

        public async Task<FacturaDTO?> FindByIdAsync(int id, int idEncargado)
        {
            var entity = await _repository.FindByIdAsync(id, idEncargado);
            if (entity is null)
            {
                return null;
            }

            var dto = _mapper.Map<FacturaDTO>(entity);
            CompletarCalculos(dto);
            return dto;
        }

        public async Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync()
        {
            var encargados = await _repository.ListEncargadosAsync();

            return encargados
                .Where(e => e.IdSucursalNavigation != null)
                .Select(e => (e.IdUsuario, $"{e.Nombre} - {e.IdSucursalNavigation!.Nombre}"))
                .ToList();
        }

        // ---------- Catálogos del formulario ----------

        public async Task<List<(int Id, string Nombre, string Correo)>> ListClientesAsync()
        {
            var clientes = await _repository.ListClientesAsync();
            return clientes.Select(c => (c.IdUsuario, c.Nombre, c.Correo)).ToList();
        }

        public async Task<List<(int Id, string Nombre, decimal Precio)>> ListProductosAsync()
        {
            var productos = await _repository.ListProductosAsync();
            return productos.Select(p => (p.IdProducto, p.Nombre, p.Precio)).ToList();
        }

        public async Task<List<(int Id, string Nombre, decimal Precio)>> ListServiciosAsync()
        {
            var servicios = await _repository.ListServiciosAsync();
            return servicios.Select(s => (s.IdServicio, s.Nombre, s.Precio)).ToList();
        }

        // ---------- Registro de factura ----------

        // Encabezado de una factura nueva: fecha actual, sucursal y encargado (no editables).
        public async Task<FacturaDTO?> PrepararNuevaAsync(int idEncargado)
        {
            var encargado = await _repository.FindEncargadoAsync(idEncargado);
            if (encargado is null || encargado.IdSucursalNavigation is null)
            {
                return null;
            }

            return new FacturaDTO
            {
                IdSucursal = encargado.IdSucursalNavigation.IdSucursal,
                SucursalNombre = encargado.IdSucursalNavigation.Nombre,
                IdEncargado = encargado.IdUsuario,
                EncargadoNombre = encargado.Nombre,
                FechaEmision = DateTime.Now,
                Estado = "Facturado",
                PorcentajeImpuesto = PorcentajeImpuesto
            };
        }

        // Precarga la información registrada en la proforma (cliente, sucursal, servicios con
        // cantidad, subtotal y total). La fecha es la actual, porque es cuando se factura.
        public async Task<FacturaDTO?> PrepararDesdeProformaAsync(int idFactura, int idEncargado)
        {
            var proforma = await _repository.FindByIdAsync(idFactura, idEncargado);
            if (proforma is null || proforma.Estado != "Proforma")
            {
                return null;
            }

            var encargado = await _repository.FindEncargadoAsync(idEncargado);
            if (encargado is null)
            {
                return null;
            }

            var dto = _mapper.Map<FacturaDTO>(proforma);
            dto.IdEncargado = encargado.IdUsuario;
            dto.EncargadoNombre = encargado.Nombre;
            dto.FechaEmision = DateTime.Now;

            CompletarCalculos(dto);
            return dto;
        }

        // Registra la factura. Si dto.IdFactura > 0 convierte esa proforma en factura.
        // Devuelve el id de la factura, o null si algún dato no es válido.
        public async Task<int?> RegistrarAsync(FacturaDTO dto, int idEncargado)
        {
            var encargado = await _repository.FindEncargadoAsync(idEncargado);
            if (encargado is null || encargado.IdSucursal is null)
            {
                return null;
            }

            // Si viene de una proforma, debe existir, ser de la sucursal del encargado y seguir en estado Proforma.
            Facturas? proforma = null;
            if (dto.IdFactura > 0)
            {
                proforma = await _repository.FindByIdAsync(dto.IdFactura, idEncargado);
                if (proforma is null || proforma.Estado != "Proforma")
                {
                    return null;
                }
            }

            // El cliente de una proforma no cambia; en una factura nueva debe ser un cliente existente.
            var idCliente = proforma?.IdCliente ?? dto.IdCliente;
            var clientes = await _repository.ListClientesAsync();
            if (!clientes.Any(c => c.IdUsuario == idCliente))
            {
                return null;
            }

            // Los precios salen de la BD, no del formulario.
            var preciosProductos = (await _repository.ListProductosAsync())
                .ToDictionary(p => p.IdProducto, p => p.Precio);
            var preciosServicios = (await _repository.ListServiciosAsync())
                .ToDictionary(s => s.IdServicio, s => s.Precio);

            // Los servicios de la proforma conservan el precio con que se registraron.
            if (proforma != null)
            {
                foreach (var linea in proforma.FacturaServicios)
                {
                    preciosServicios[linea.IdServicio] = linea.PrecioUnitario;
                }
            }

            var factura = new Facturas
            {
                IdFactura = dto.IdFactura,
                IdCita = proforma?.IdCita,
                IdCliente = idCliente,
                IdSucursal = encargado.IdSucursal.Value,
                IdEncargado = encargado.IdUsuario,
                FechaEmision = DateTime.Now,
                IdMetodoPago = dto.IdMetodoPago,
                Estado = "Facturado"
            };

            // Las líneas con cantidad 0 se descartan (equivale a borrarlas).
            foreach (var p in dto.FacturaProductos.Where(p => p.Cantidad > 0))
            {
                if (!preciosProductos.TryGetValue(p.IdProducto, out var precio))
                {
                    return null;
                }

                factura.FacturaProductos.Add(new FacturaProductos
                {
                    IdProducto = p.IdProducto,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = precio,
                    Subtotal = precio * p.Cantidad
                });
            }

            foreach (var s in dto.FacturaServicios.Where(s => s.Cantidad > 0))
            {
                if (!preciosServicios.TryGetValue(s.IdServicio, out var precio))
                {
                    return null;
                }

                factura.FacturaServicios.Add(new FacturaServicios
                {
                    IdServicio = s.IdServicio,
                    Cantidad = s.Cantidad,
                    PrecioUnitario = precio,
                    Subtotal = precio * s.Cantidad
                });
            }

            if (factura.FacturaProductos.Count == 0 && factura.FacturaServicios.Count == 0)
            {
                return null;
            }

            factura.Subtotal = factura.FacturaProductos.Sum(p => p.Subtotal)
                             + factura.FacturaServicios.Sum(s => s.Subtotal);

            factura.Impuesto = factura.FacturaProductos.Sum(p => Math.Round(p.Subtotal * PorcentajeImpuesto, 2))
                             + factura.FacturaServicios.Sum(s => Math.Round(s.Subtotal * PorcentajeImpuesto, 2));

            factura.Total = factura.Subtotal + factura.Impuesto;

            if (proforma != null)
            {
                var actualizada = await _repository.UpdateAsync(factura);
                return actualizada ? factura.IdFactura : null;
            }

            await _repository.AddAsync(factura);
            return factura.IdFactura;
        }

        // Impuesto de cada línea y porcentaje que usa la vista para el cálculo en tiempo real.
        private static void CompletarCalculos(FacturaDTO dto)
        {
            dto.PorcentajeImpuesto = PorcentajeImpuesto;

            foreach (var p in dto.FacturaProductos)
            {
                p.Impuesto = Math.Round(p.Subtotal * PorcentajeImpuesto, 2);
            }
            foreach (var s in dto.FacturaServicios)
            {
                s.Impuesto = Math.Round(s.Subtotal * PorcentajeImpuesto, 2);
            }
        }
    }
}