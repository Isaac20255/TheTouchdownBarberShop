using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;


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
            // Obtener la entidad del repositorio (null si no existe o no pertenece a la sucursal del encargado)
            var @object = await _repository.FindByIdAsync(id, idEncargado);

            // Map Citas a CitaDTO (si es null, el resultado también es null)
            var objectMapped = _mapper.Map<CitaDTO?>(@object);

            return objectMapped;
        }

        public async Task<ICollection<CitaDTO>> ListAsync(int idEncargado)
        {
            // Obtener datos del repositorio (solo las citas de la sucursal del encargado)
            var list = await _repository.ListAsync(idEncargado);

            // Map ICollection<Citas> a ICollection<CitaDTO>
            var collection = _mapper.Map<ICollection<CitaDTO>>(list);

            // Return lista
            return collection;
        }

        public async Task<List<(int IdUsuario, string Descripcion)>> ListEncargadosAsync()
        {
            // Encargados desde la BD: "Nombre - Sucursal"
            var encargados = await _repository.ListEncargadosAsync();

            return encargados
                .Select(e => (e.IdUsuario, $"{e.Nombre} - {e.IdSucursalNavigation!.Nombre}"))
                .ToList();
        }
    }
}