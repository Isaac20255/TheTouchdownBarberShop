using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IMapper _mapper;

        public ServiceProducto(IRepositoryProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<ProductoDTO>>(list);
        }

        public async Task<ProductoDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<ProductoDTO?>(@object);
        }

        public async Task<ICollection<CategoriasProductos>> ListCategoriasAsync()
        {
            return await _repository.ListCategoriasAsync();
        }

        public async Task AddAsync(ProductoDTO productoDto)
        {
            var entity = _mapper.Map<Producto>(productoDto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ProductoDTO productoDto)
        {
            var existente = await _repository.FindByIdAsync(productoDto.IdProducto);
            var entity = _mapper.Map<Producto>(productoDto);

            if (string.IsNullOrEmpty(entity.Imagen) && existente != null)
            {
                entity.Imagen = existente.Imagen;
            }

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
           await _repository.DeleteAsync(id);
        }
    }
}