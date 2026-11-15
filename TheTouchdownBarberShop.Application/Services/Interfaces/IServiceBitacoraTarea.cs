using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.Services
{
    public interface IServiceBitacoraTarea
    {
        Task<ICollection<BitacoraTareaDTO>> ListarBitacoraAsync();
    }
}