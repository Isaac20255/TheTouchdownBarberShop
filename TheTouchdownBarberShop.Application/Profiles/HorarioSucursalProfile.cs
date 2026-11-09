using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class HorarioSucursalProfile : Profile
    {
        public HorarioSucursalProfile()
        {
            CreateMap<HorariosSucursal, HorarioSucursalDTO>()
                .ForMember(d => d.SucursalNombre,
                    o => o.MapFrom(s => s.IdSucursalNavigation != null
                                        ? s.IdSucursalNavigation.Nombre
                                        : null));

            CreateMap<HorarioSucursalDTO, HorariosSucursal>()
                .ForMember(d => d.IdSucursalNavigation, o => o.Ignore());
        }
    }
}