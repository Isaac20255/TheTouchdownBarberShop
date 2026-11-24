using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Application.Profiles
{
    public class CitaProfile : Profile
    {
        public CitaProfile()
        {
            // Entidad -> DTO (listado, detalle y precarga de edición)
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
                        : new List<string>()))
                .ForMember(d => d.IdsServicios, o => o.MapFrom(s =>
                    s.CitaServicios != null
                        ? s.CitaServicios.Select(cs => cs.IdServicio).ToList()
                        : new List<int>()))
                .ForMember(d => d.Respuestas, o => o.MapFrom(s =>
                    s.CitaPreguntas != null
                        ? s.CitaPreguntas
                            .GroupBy(p => p.Pregunta ?? "")
                            .ToDictionary(g => g.Key, g => g.First().Respuesta)
                        : new Dictionary<string, string?>()));

            // DTO -> entidad (solo escalares; los hijos y los campos del servidor los arma el servicio)
            CreateMap<CitaDTO, Citas>()
                .ForMember(d => d.CitaServicios, o => o.Ignore())
                .ForMember(d => d.CitaPreguntas, o => o.Ignore())
                .ForMember(d => d.IdSucursal, o => o.Ignore())
                .ForMember(d => d.Estado, o => o.Ignore())
                .ForMember(d => d.FechaCreacion, o => o.Ignore());
        }
    }
}