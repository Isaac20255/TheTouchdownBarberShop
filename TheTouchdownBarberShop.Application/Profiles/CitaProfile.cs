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
                .ForMember(d => d.NombreCliente, o => o.MapFrom(s =>
                    s.IdClienteNavigation != null ? s.IdClienteNavigation.Nombre : "Cliente no disponible"))
                .ForMember(d => d.NombreBarbero, o => o.MapFrom(s =>
                    s.IdBarberoNavigation != null ? s.IdBarberoNavigation.Nombre : "Sin asignar"))
                .ForMember(d => d.Sucursal, o => o.MapFrom(s =>
                    s.IdSucursalNavigation != null ? s.IdSucursalNavigation.Nombre : "Sucursal no disponible"))
                .ForMember(d => d.Paquete, o => o.MapFrom(s =>
                    s.IdPaqueteNavigation != null ? s.IdPaqueteNavigation.Nombre : null))
                .ForMember(d => d.Total, o => o.MapFrom(s =>
                    s.CitaServicios != null ? s.CitaServicios.Sum(cs => cs.PrecioAplicado) : 0m))
                .ForMember(d => d.Servicios, o => o.MapFrom(s =>
                    s.CitaServicios != null
                        ? s.CitaServicios.Select(cs =>
                            $"{(cs.IdServicioNavigation != null ? cs.IdServicioNavigation.Nombre : "Servicio")} - ₡{cs.PrecioAplicado:N2}").ToList()
                        : new List<string>()))
                .ForMember(d => d.Preguntas, o => o.MapFrom(s =>
                    s.CitaPreguntas != null
                        ? s.CitaPreguntas.Select(p => $"{p.Pregunta ?? "Pregunta"}: {p.Respuesta ?? "Sin respuesta"}").ToList()
                        : new List<string>()));
        }
    }
}