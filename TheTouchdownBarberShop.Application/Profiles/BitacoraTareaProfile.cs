using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class BitacoraTareaProfile : Profile
    {
        public BitacoraTareaProfile()
        {
            CreateMap<BitacoraTarea, BitacoraTareaDTO>()
                .ForMember(dest => dest.ProductoNavigation, opt => opt.MapFrom(src => src.IdProductoNavigation))
                .ReverseMap();
        }
    }
}