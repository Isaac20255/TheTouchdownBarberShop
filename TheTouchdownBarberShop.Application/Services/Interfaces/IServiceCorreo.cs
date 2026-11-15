namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceCorreo
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string mensaje);
    }
}