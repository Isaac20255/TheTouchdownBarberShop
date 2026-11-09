using AutoMapper;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class SucursalProfile : Profile
    {
        public SucursalProfile()
        {
            CreateMap<Sucursal, SucursalDTO>()
                .ForMember(dest => dest.AdministradorNombre, opt => opt.MapFrom(src => src.Administrador != null ? src.Administrador.Nombre : string.Empty));

            CreateMap<SucursalDTO, Sucursal>();
        }
    }
}