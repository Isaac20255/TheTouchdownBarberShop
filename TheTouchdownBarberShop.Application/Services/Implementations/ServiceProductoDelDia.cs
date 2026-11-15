using Microsoft.Extensions.Logging;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceProductoDelDia : IServiceProductoDelDia
    {
        private readonly IRepositoryTarea _repositoryTarea;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IServiceCorreo _serviceCorreo;
        private readonly ILogger<ServiceProductoDelDia> _logger;

        public ServiceProductoDelDia(
            IRepositoryTarea repositoryTarea,
            IRepositoryProducto repositoryProducto,
            IServiceCorreo serviceCorreo,
            ILogger<ServiceProductoDelDia> logger)
        {
            _repositoryTarea = repositoryTarea;
            _repositoryProducto = repositoryProducto;
            _serviceCorreo = serviceCorreo;
            _logger = logger;
        }

        public async Task EjecutarTareaAsync()
        {
            try
            {
                string nombreTarea = "ProductoDelDia";
                var fechaHoy = DateOnly.FromDateTime(DateTime.Now);
                var horaActual = TimeOnly.FromDateTime(DateTime.Now);

                var config = await _repositoryTarea.ObtenerConfiguracionPorNombreAsync(nombreTarea);
                if (config == null)
                {
                    _logger.LogWarning("La configuración para 'ProductoDelDia' es NULL.");
                    return;
                }
                if (!config.Activa)
                {
                    _logger.LogWarning("La tarea 'ProductoDelDia' está inactiva.");
                    return;
                }
                // Comentado temporalmente para pruebas inmediatas:
                // if (horaActual < config.HoraEjecucion) return;

                if (await _repositoryTarea.ExisteEjecucionHoyAsync(nombreTarea, fechaHoy)) return;

                var productosActivos = await _repositoryProducto.ListarActivosAsync();
                if (productosActivos == null || !productosActivos.Any()) return;

                var productoAnterior = await _repositoryProducto.ObtenerProductoDelDiaAsync(fechaHoy.AddDays(-1));
                var candidatos = productosActivos.Where(p => productoAnterior == null || p.IdProducto != productoAnterior.IdProducto).ToList();
                if (!candidatos.Any()) candidatos = productosActivos.ToList();

                var random = new Random();
                var seleccionado = candidatos[random.Next(candidatos.Count)];

                await _repositoryProducto.MarcarProductoDelDiaAsync(seleccionado.IdProducto, fechaHoy);

                bool correoEnviado = true;
                try
                {
                    string asunto = $"🔥 ¡Producto del Día: {seleccionado.Nombre}!";
                    string cuerpo = $"<h2>Nuevo Producto del Día</h2><p>{seleccionado.Nombre}</p><p>{seleccionado.Descripcion}</p><p><b>Precio:</b> ₡{seleccionado.Precio:N2}</p>";
                    await _serviceCorreo.EnviarCorreoAsync(config.CorreoDestino, asunto, cuerpo);
                }
                catch (Exception ex)
                {
                    correoEnviado = false;
                    _logger.LogError(ex, "Error al enviar correo del producto del día.");
                }

                await _repositoryTarea.RegistrarBitacoraAsync(new BitacoraTarea
                {
                    NombreTarea = nombreTarea,
                    FechaEjecucion = DateTime.Now,
                    IdProducto = seleccionado.IdProducto,
                    CorreoEnviado = correoEnviado,
                    Resultado = correoEnviado ? "Completado con éxito" : "Producto marcado pero falló el envío del correo"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la ejecución de la tarea Producto del Día.");
            }
        }
    }
}