using TheTouchdownBarberShop.Application.DTOs;

namespace TheTouchdownBarberShop.Application.Services.Interfaces
{
    public interface IServiceHorarioSucursal
    {
        Task<ICollection<HorarioSucursalDTO>> ListBySucursalAsync(int idSucursal);

        Task<HorarioSucursalDTO?> FindByIdAsync(int id);

        Task AddAsync(HorarioSucursalDTO horarioDto);

        Task<bool> EstaAbiertaAsync(int idSucursal);
    }
}