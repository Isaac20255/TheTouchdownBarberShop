using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceHorarioSucursal : IServiceHorarioSucursal
    {
        private readonly IRepositoryHorarioSucursal _repository;
        private readonly IMapper _mapper;

        public ServiceHorarioSucursal(IRepositoryHorarioSucursal repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<HorarioSucursalDTO>> ListBySucursalAsync(int idSucursal)
        {
            var list = await _repository.ListBySucursalAsync(idSucursal);
            return _mapper.Map<ICollection<HorarioSucursalDTO>>(list);
        }

        public async Task<HorarioSucursalDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<HorarioSucursalDTO?>(@object);
        }

        public async Task AddAsync(HorarioSucursalDTO horarioDto)
        {
            if (horarioDto.DiasSeleccionados == null || horarioDto.DiasSeleccionados.Count == 0)
            {
                throw new InvalidOperationException("Debe seleccionar al menos un día de la semana.");
            }

            if (horarioDto.HoraFin <= horarioDto.HoraInicio)
            {
                throw new InvalidOperationException("La hora de fin debe ser posterior a la hora de inicio.");
            }

            // Primero se validan todos los días, para no guardar solo una parte si alguno falla.
            foreach (var dia in horarioDto.DiasSeleccionados)
            {
                var haySobreposicion = await _repository.ExisteSobreposicionAsync(
                    horarioDto.IdSucursal, dia, horarioDto.HoraInicio, horarioDto.HoraFin);

                if (haySobreposicion)
                {
                    throw new InvalidOperationException("Existe una sobreposición de horarios el día " + dia + " en esa sucursal.");
                }
            }

            foreach (var dia in horarioDto.DiasSeleccionados)
            {
                var entity = new HorariosSucursal
                {
                    IdSucursal = horarioDto.IdSucursal,
                    DiaSemana = dia,
                    HoraInicio = horarioDto.HoraInicio,
                    HoraFin = horarioDto.HoraFin
                };
                await _repository.AddAsync(entity);
            }
        }

        public async Task<bool> EstaAbiertaAsync(int idSucursal)
        {
            var ahora = DateTime.Now;

            var dia = ahora.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                _ => "Domingo"
            };

            return await _repository.EstaAbiertaAsync(idSucursal, dia, TimeOnly.FromDateTime(ahora));
        }
    }
}