using AutoMapper;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceSucursal : IServiceSucursal
    {
        private readonly IRepositorySucursal _repository;
        private readonly IMapper _mapper;

        public ServiceSucursal(IRepositorySucursal repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SucursalDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<SucursalDTO?>(@object);
        }

        public async Task<ICollection<SucursalDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<SucursalDTO>>(list);
        }

        public async Task AddAsync(SucursalDTO sucursalDto)
        {
            var entity = _mapper.Map<Infraestructure.Models.Sucursal>(sucursalDto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(SucursalDTO sucursalDto)
        {
            var entity = _mapper.Map<Infraestructure.Models.Sucursal>(sucursalDto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}