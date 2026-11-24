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
                           opt => opt.MapFrom(src => src.IdClienteNavigation != null ? src.IdClienteNavigation.Nombre : null))
                .ForMember(dest => dest.ClienteCorreo,
                           opt => opt.MapFrom(src => src.IdClienteNavigation != null ? src.IdClienteNavigation.Correo : null))
                .ForMember(dest => dest.SucursalNombre,
                           opt => opt.MapFrom(src => src.IdSucursalNavigation != null ? src.IdSucursalNavigation.Nombre : null))
                .ForMember(dest => dest.EncargadoNombre,
                           opt => opt.MapFrom(src => src.IdEncargadoNavigation != null ? src.IdEncargadoNavigation.Nombre : "No registrado"))
                .ForMember(dest => dest.MetodoPagoNombre,
                           opt => opt.MapFrom(src => src.IdMetodoPagoNavigation != null ? src.IdMetodoPagoNavigation.Nombre : "No definido"))
                .ForMember(dest => dest.EstadoCita,
                           opt => opt.MapFrom(src => src.IdCitaNavigation != null ? src.IdCitaNavigation.Estado : null))
                .ForMember(dest => dest.PorcentajeImpuesto, opt => opt.Ignore());

            CreateMap<FacturaProductos, FacturaProductoDTO>()
                .ForMember(dest => dest.ProductoNombre,
                           opt => opt.MapFrom(src => src.IdProductoNavigation != null ? src.IdProductoNavigation.Nombre : null))
                .ForMember(dest => dest.Impuesto, opt => opt.Ignore());

            CreateMap<FacturaServicios, FacturaServicioDTO>()
                .ForMember(dest => dest.ServicioNombre,
                           opt => opt.MapFrom(src => src.IdServicioNavigation != null ? src.IdServicioNavigation.Nombre : null))
                .ForMember(dest => dest.Impuesto, opt => opt.Ignore());
        }
    }
}