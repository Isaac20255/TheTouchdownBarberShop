using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Application.Services.Implementations
{
    public class ServiceProductoDelDiaBackground : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ServiceProductoDelDiaBackground> _logger;
        private readonly TimeSpan _periodo = TimeSpan.FromMinutes(1); // Revisa cada minuto

        public ServiceProductoDelDiaBackground(
            IServiceProvider serviceProvider,
            ILogger<ServiceProductoDelDiaBackground> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_periodo);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IServiceProductoDelDia>();
                    await service.EjecutarTareaAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el Background Service del Producto del Día.");
                }
            }
        }
    }
}