using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Implementations;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ProductoDTO?> FindByIdAsync(int id)
        {
            // Obtener la entidad del repositorio (null si no existe o está inactivo)
            var @object = await _repository.FindByIdAsync(id);

            // Map Productos a ProductoDTO (si es null, el resultado también es null)
            var objectMapped = _mapper.Map<ProductoDTO?>(@object);

            return objectMapped;
        }

        public async Task<ICollection<ProductoDTO>> ListAsync()
        {
            // Obtener datos del repositorio
            var list = await _repository.ListAsync();

            // Map ICollection<Productos> a ICollection<ProductoDTO>
            var collection = _mapper.Map<ICollection<ProductoDTO>>(list);

            // Return lista
            return collection;
        }
    }
}