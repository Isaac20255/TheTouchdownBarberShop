using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceCita : IServiceCita
    {
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
    }
}