using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class CitaProfile : Profile
    {

        public CitaProfile()
        {
            CreateMap<Citas, CitaDTO>()
     .ForMember(d => d.NombreCliente, o => o.MapFrom(s => s.IdClienteNavigation.Nombre))
     .ForMember(d => d.NombreBarbero, o => o.MapFrom(s =>
         s.IdBarberoNavigation != null ? s.IdBarberoNavigation.Nombre : "Sin asignar"))
     .ForMember(d => d.Sucursal, o => o.MapFrom(s => s.IdSucursalNavigation.Nombre))
     .ForMember(d => d.Paquete, o => o.MapFrom(s =>
         s.IdPaqueteNavigation != null ? s.IdPaqueteNavigation.Nombre : null))
     .ForMember(d => d.Total, o => o.MapFrom(s => s.CitaServicios.Sum(cs => cs.PrecioAplicado)))
     .ForMember(d => d.Servicios, o => o.MapFrom(s =>
         s.CitaServicios.Select(cs => cs.IdServicioNavigation.Nombre + " - ₡" + cs.PrecioAplicado.ToString("N2"))))
     .ForMember(d => d.Preguntas, o => o.MapFrom(s =>
         s.CitaPreguntas.Select(p => p.Pregunta + ": " + p.Respuesta)));
        }
    }
}
