using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class ServicioProfile : Profile
    {
        public ServicioProfile()
        {
            CreateMap<Servicios, ServicioDTO>()
                .ForMember(d => d.NombreCategoria, o => o.MapFrom(s => s.IdCategoriaNavigation != null ? s.IdCategoriaNavigation.Nombre : null))
                .ForMember(d => d.Imagen, o => o.MapFrom(s => s.Imagen));

            CreateMap<ServicioDTO, Servicios>()
                .ForMember(d => d.Imagen, o => o.MapFrom(s => s.Imagen))
                .ForMember(d => d.IdCategoriaNavigation, o => o.Ignore());
        }
    }
}