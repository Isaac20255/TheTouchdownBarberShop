using TheTouchdownBarberShop.Infraestructure.Models;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryHorarioSucursal
    {
        Task<ICollection<HorariosSucursal>> ListBySucursalAsync(int idSucursal);
        Task<HorariosSucursal?> FindByIdAsync(int id);
        Task AddAsync(HorariosSucursal horario);
        Task<bool> ExisteSobreposicionAsync(int idSucursal, string diaSemana, TimeOnly horaInicio, TimeOnly horaFin);
        Task<bool> EstaAbiertaAsync(int idSucursal, string diaSemana, TimeOnly hora);
    }
}