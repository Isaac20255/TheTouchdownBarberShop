using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(d => d.NombreRol, o => o.MapFrom(s => s.IdRolNavigation.Nombre));
    }
}