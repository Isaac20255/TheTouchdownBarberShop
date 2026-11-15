using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services
{
    public class ServiceBitacoraTarea : IServiceBitacoraTarea
    {
        private readonly IRepositoryTarea _repositoryTarea;
        private readonly IMapper _mapper;

        public ServiceBitacoraTarea(IRepositoryTarea repositoryTarea, IMapper mapper)
        {
            _repositoryTarea = repositoryTarea;
            _mapper = mapper;
        }

        public async Task<ICollection<BitacoraTareaDTO>> ListarBitacoraAsync()
        {
            var bitacoras = await _repositoryTarea.ListarBitacoraAsync();
            return _mapper.Map<ICollection<BitacoraTareaDTO>>(bitacoras);
        }
    }
}