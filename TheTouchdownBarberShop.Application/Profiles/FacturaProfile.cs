using AutoMapper;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class FacturaProfile : Profile
    {
        public FacturaProfile()
        {
            CreateMap<Facturas, FacturaDTO>()
                .ForMember(dest => dest.ClienteNombre,
                           opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre))
                .ForMember(dest => dest.MetodoPagoNombre,
                           opt => opt.MapFrom(src => src.IdMetodoPagoNavigation.Nombre));

            CreateMap<FacturaProductos, FacturaProductoDTO>()
                .ForMember(dest => dest.ProductoNombre,
                           opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre));
        }
    }
}