using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceServicio : IServiceServicio
    {
        private readonly IRepositoryServicio _repository;
        private readonly IMapper _mapper;

        public ServiceServicio(IRepositoryServicio repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ServicioDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<ServicioDTO>>(list);
        }

        public async Task<ServicioDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<ServicioDTO?>(@object);
        }

        public async Task<ICollection<CategoriasServicios>> ListCategoriasAsync()
        {
            return await _repository.ListCategoriasAsync();
        }

        public async Task AddAsync(ServicioDTO servicioDto)
        {
            var entity = _mapper.Map<Servicios>(servicioDto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ServicioDTO servicioDto)
        {
            var entity = _mapper.Map<Servicios>(servicioDto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}