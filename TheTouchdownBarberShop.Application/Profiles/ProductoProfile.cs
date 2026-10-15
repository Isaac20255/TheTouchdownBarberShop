using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Productos, ProductoDTO>()
                .ForMember(dest => dest.CategoriaNombre,
                           opt => opt.MapFrom(src => src.IdCategoriaProductoNavigation.Nombre));
        }
    }
}