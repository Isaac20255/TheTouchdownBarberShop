using TheTouchdownBarberShop.Application.DTO;


namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceFactura
    {

        Task<ICollection<FacturaDTO>> ListAsync();

        Task<FacturaDTO?> FindByIdAsync(int id);


    }
}
