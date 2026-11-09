using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;
using TheTouchdownBarberShop.Infraestructure.Repository.Interfaces;

namespace TheTouchdownBarberShop.Infraestructure.Repository.Implementations
{
    public class RepositoryHorarioSucursal : IRepositoryHorarioSucursal
    {
        private readonly TheTouchdownBarberShopContext _context;

        public RepositoryHorarioSucursal(TheTouchdownBarberShopContext context)
        {
            _context = context;
        }

        public async Task<ICollection<HorariosSucursal>> ListBySucursalAsync(int idSucursal)
        {
            return await _context.Set<HorariosSucursal>()
                .AsNoTracking()
                .Include(h => h.IdSucursalNavigation)
                .Where(h => h.IdSucursal == idSucursal)
                .OrderBy(h => h.HoraInicio)
                .ToListAsync();
        }

        public async Task<HorariosSucursal?> FindByIdAsync(int id)
        {
            return await _context.Set<HorariosSucursal>()
                .AsNoTracking()
                .Include(h => h.IdSucursalNavigation)
                .FirstOrDefaultAsync(h => h.IdHorarioSucursal == id);
        }

        public async Task AddAsync(HorariosSucursal horario)
        {
            await _context.Set<HorariosSucursal>().AddAsync(horario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteSobreposicionAsync(int idSucursal, string diaSemana, TimeOnly horaInicio, TimeOnly horaFin)
        {
            return await _context.Set<HorariosSucursal>()
                .AnyAsync(h => h.IdSucursal == idSucursal
                            && h.DiaSemana == diaSemana
                            && h.HoraInicio < horaFin
                            && h.HoraFin > horaInicio);
        }

        public async Task<bool> EstaAbiertaAsync(int idSucursal, string diaSemana, TimeOnly hora)
        {
            return await _context.Set<HorariosSucursal>()
                .AnyAsync(h => h.IdSucursal == idSucursal
                            && h.DiaSemana == diaSemana
                            && h.HoraInicio <= hora
                            && h.HoraFin > hora);
        }
    }
}