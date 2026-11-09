using TheTouchdownBarberShop.Application.DTO;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceSucursal
    {
        Task<ICollection<SucursalDTO>> ListAsync();
        Task<SucursalDTO?> FindByIdAsync(int id);
        Task AddAsync(SucursalDTO sucursalDto);
        Task UpdateAsync(SucursalDTO sucursalDto);
        Task DeleteAsync(int id);
    }
}