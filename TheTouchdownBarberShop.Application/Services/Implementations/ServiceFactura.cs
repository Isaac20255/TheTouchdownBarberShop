using AutoMapper;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Repository.Implementations;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceFactura : IServiceFactura
    {
        private readonly IRepositoryFactura _repository;
        private readonly IMapper _mapper;

        public ServiceFactura(IRepositoryFactura repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

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
    }
}