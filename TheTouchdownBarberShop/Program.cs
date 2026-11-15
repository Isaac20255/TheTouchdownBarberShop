using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;
using TheTouchdownBarberShop.Application.Profiles;
using TheTouchdownBarberShop.Application.Services;
using TheTouchdownBarberShop.Application.Services.Implementations;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Repository.Implementations;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;
using TheTouchdownBarberShop.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar D.I.
// Repository
builder.Services.AddTransient<IRepositoryProducto, RepositoryProducto>();
builder.Services.AddTransient<IRepositoryFactura, RepositoryFactura>();
builder.Services.AddScoped<IRepositoryCita, RepositoryCita>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddScoped<IRepositoryServicio, RepositoryServicio>();
builder.Services.AddScoped<IRepositorySucursal, RepositorySucursal>();
builder.Services.AddScoped<IRepositoryHorarioSucursal, RepositoryHorarioSucursal>();
builder.Services.AddScoped<IRepositoryTarea, RepositoryTarea>();

// Services
builder.Services.AddTransient<IServiceProducto, ServiceProducto>();
builder.Services.AddTransient<IServiceFactura, ServiceFactura>();
builder.Services.AddScoped<IServiceCita, ServiceCita>();
builder.Services.AddScoped<IServiceUsuario, ServiceUsuario>();
builder.Services.AddScoped<IServiceServicio, ServiceServicio>();
builder.Services.AddScoped<IServiceSucursal, ServiceSucursal>();
builder.Services.AddScoped<IServiceHorarioSucursal, ServiceHorarioSucursal>();

//Tarea Programada
builder.Services.AddScoped<IServiceBitacoraTarea, ServiceBitacoraTarea>();
builder.Services.AddScoped<IServiceProductoDelDia, ServiceProductoDelDia>();
builder.Services.AddScoped<IServiceCorreo, ServiceCorreo>();
builder.Services.AddHostedService<ServiceProductoDelDiaBackground>();

// Configurar Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductoProfile>();
    config.AddProfile<FacturaProfile>();
    config.AddProfile<CitaProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<ServicioProfile>();
    config.AddProfile<SucursalProfile>();
    config.AddProfile<HorarioSucursalProfile>();
    config.AddProfile<BitacoraTareaProfile>();
});

// Configurar Conexión a la Base de Datos SQL 
builder.Services.AddDbContext<TheTouchdownBarberShopContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Configuración Serilog
var logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Console(LogEventLevel.Information)
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
    .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

// Activar soporte a la solicitud de registro con SERILOG 
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activar Antiforgery  
app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();