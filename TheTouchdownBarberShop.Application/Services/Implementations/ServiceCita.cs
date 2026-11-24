using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceCita : IServiceCita
    {
        private const decimal PorcentajeImpuesto = 0.13m;

        private readonly IRepositoryCita _repository;
        private readonly IMapper _mapper;

        public ServiceCita(IRepositoryCita repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CitaDTO?> FindByIdAsync(int id, int idEncargado)
        {
            var entity = await _repository.FindByIdAsync(id, idEncargado);
            return entity is null ? null : _mapper.Map<CitaDTO>(entity);
        }

        public async Task<ICollection<CitaDTO>> ListAsync(int idEncargado, string? cliente = null, DateTime? fecha = null)
        {
            var list = await _repository.ListAsync(idEncargado);
            var mapped = _mapper.Map<List<CitaDTO>>(list);

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                var termino = cliente.Trim();
                mapped = mapped
                    .Where(c => !string.IsNullOrEmpty(c.NombreCliente) &&
                                c.NombreCliente.Contains(termino, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (fecha.HasValue)
            {
                var fechaBuscada = fecha.Value.Date;
                mapped = mapped
                    .Where(c => c.FechaHora.Date == fechaBuscada)
                    .ToList();
            }

            return mapped;
        }

        public async Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync()
        {
            var encargados = await _repository.ListEncargadosAsync();

            return encargados
                .Where(e => e.IdSucursalNavigation != null)
                .Select(e => (e.IdUsuario, $"{e.Nombre} - {e.IdSucursalNavigation!.Nombre}"))
                .ToList();
        }

        public async Task<List<(int Id, string Descripcion)>> ListClientesAsync()
        {
            var clientes = await _repository.ListUsuariosPorRolAsync("Cliente");
            return clientes.Select(c => (c.IdUsuario, c.Nombre)).ToList();
        }

        public async Task<List<(int Id, string Descripcion)>> ListBarberosAsync()
        {
            var barberos = await _repository.ListUsuariosPorRolAsync("Barbero");
            return barberos.Select(b => (b.IdUsuario, b.Nombre)).ToList();
        }

        public async Task<List<(int Id, string Descripcion)>> ListPaquetesAsync()
        {
            var paquetes = await _repository.ListPaquetesAsync();
            return paquetes.Select(p => (p.IdPaquete, p.Nombre)).ToList();
        }

        public async Task<List<(int Id, string Descripcion)>> ListServiciosAsync()
        {
            var servicios = await _repository.ListServiciosAsync();
            return servicios.Select(s => (s.IdServicio, $"{s.Nombre} - ₡{s.Precio:N2}")).ToList();
        }

        public async Task<int?> AddAsync(CitaDTO dto, int idEncargado)
        {
            var idSucursal = await _repository.ObtenerSucursalEncargadoAsync(idEncargado);
            if (!idSucursal.HasValue)
            {
                return null;
            }

            var entity = _mapper.Map<Citas>(dto);
            entity.IdCita = 0;
            entity.IdSucursal = idSucursal.Value;
            entity.Estado = "Pendiente";
            entity.FechaCreacion = DateTime.Now;

            var hijosCargados = await CargarHijosAsync(entity, dto, preciosExistentes: null);
            if (!hijosCargados)
            {
                return null;
            }

            entity.Facturas = ConstruirProformaDesdeCita(entity, idEncargado, DateTime.Now);

            await _repository.AddAsync(entity);
            return entity.IdCita;
        }

        public async Task<bool> UpdateAsync(CitaDTO dto, int idEncargado)
        {
            // Confirma que la cita existe y pertenece a la sucursal del encargado.
            var existente = await _repository.FindByIdAsync(dto.IdCita, idEncargado);
            if (existente is null)
            {
                return false;
            }

            var entity = _mapper.Map<Citas>(dto);
            entity.IdSucursal = existente.IdSucursal;
            entity.Estado = existente.Estado;
            entity.FechaCreacion = existente.FechaCreacion;

            // Los servicios que ya estaban en la cita conservan el precio con que se registraron.
            var preciosExistentes = existente.CitaServicios
                .GroupBy(cs => cs.IdServicio)
                .ToDictionary(g => g.Key, g => g.First().PrecioAplicado);

            var hijosCargados = await CargarHijosAsync(entity, dto, preciosExistentes);
            if (!hijosCargados)
            {
                return false;
            }

            if (existente.Estado != "Completada" &&
                (existente.Facturas == null || existente.Facturas.Estado == "Proforma"))
            {
                entity.Facturas = ConstruirProformaDesdeCita(
                    entity,
                    idEncargado,
                    existente.Facturas?.FechaEmision ?? DateTime.Now);
            }

            await _repository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int idEncargado)
        {
            var existente = await _repository.FindByIdAsync(id, idEncargado);
            if (existente is null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }

        private static Facturas ConstruirProformaDesdeCita(Citas cita, int idEncargado, DateTime fechaEmision)
        {
            var lineas = cita.CitaServicios
                .Select(cs => new FacturaServicios
                {
                    IdServicio = cs.IdServicio,
                    Cantidad = 1,
                    PrecioUnitario = cs.PrecioAplicado,
                    Subtotal = cs.PrecioAplicado
                })
                .ToList();

            var subtotal = lineas.Sum(l => l.Subtotal);
            var impuesto = lineas.Sum(l => Math.Round(l.Subtotal * PorcentajeImpuesto, 2));
            var total = subtotal + impuesto;

            return new Facturas
            {
                IdCita = cita.IdCita > 0 ? cita.IdCita : null,
                IdCliente = cita.IdCliente,
                IdSucursal = cita.IdSucursal,
                IdEncargado = idEncargado,
                FechaEmision = fechaEmision,
                Subtotal = subtotal,
                Impuesto = impuesto,
                Total = total,
                Estado = "Proforma",
                FacturaServicios = lineas
            };
        }

        // Arma CitaServicios y CitaPreguntas a partir del DTO.
        // Devuelve false si algún servicio seleccionado no existe en la BD.
        private async Task<bool> CargarHijosAsync(
            Citas entity,
            CitaDTO dto,
            Dictionary<int, decimal>? preciosExistentes)
        {
            var idsServicios = dto.IdsServicios.Distinct().ToList();

            // Solo se consultan los precios de los servicios que no estaban en la cita.
            var idsNuevos = preciosExistentes is null
                ? idsServicios
                : idsServicios.Where(id => !preciosExistentes.ContainsKey(id)).ToList();

            var preciosNuevos = idsNuevos.Count > 0
                ? await _repository.ObtenerPreciosServiciosAsync(idsNuevos)
                : new Dictionary<int, decimal>();

            foreach (var idServicio in idsServicios)
            {
                decimal precio;
                if (preciosExistentes != null && preciosExistentes.TryGetValue(idServicio, out var precioGuardado))
                {
                    precio = precioGuardado;
                }
                else if (!preciosNuevos.TryGetValue(idServicio, out precio))
                {
                    return false;
                }

                entity.CitaServicios.Add(new CitaServicios
                {
                    IdServicio = idServicio,
                    PrecioAplicado = precio
                });
            }

            foreach (var r in dto.Respuestas.Where(r => !string.IsNullOrWhiteSpace(r.Value)))
            {
                entity.CitaPreguntas.Add(new CitaPreguntas
                {
                    Pregunta = r.Key,
                    Respuesta = r.Value
                });
            }

            return true;
        }

        public async Task<string?> ObtenerNombreSucursalEncargadoAsync(int idEncargado)
        {
            return await _repository.ObtenerNombreSucursalEncargadoAsync(idEncargado);
        }
    }
}