
using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>()
       .ForMember(d => d.CategoriaNombre,
           o => o.MapFrom(s => s.IdCategoriaProductoNavigation != null
                               ? s.IdCategoriaProductoNavigation.Nombre
                               : null));

            CreateMap<ProductoDTO, Producto>()
                .ForMember(d => d.IdCategoriaProductoNavigation, o => o.Ignore());
        }
    }
}
