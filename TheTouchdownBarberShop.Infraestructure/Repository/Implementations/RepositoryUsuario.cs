using Microsoft.EntityFrameworkCore;
using TheTouchdownBarberShop.Infraestructure.Data;
using TheTouchdownBarberShop.Infraestructure.Models;

public class RepositoryUsuario : IRepositoryUsuario
{
    private readonly TheTouchdownBarberShopContext _context;
    public RepositoryUsuario(TheTouchdownBarberShopContext context) => _context = context;

    public async Task<ICollection<Usuario>> ListAsync() =>
        await _context.Usuarios
            .Include(u => u.IdRolNavigation)    
            .AsNoTracking()
            .OrderBy(u => u.IdUsuario)
            .ToListAsync();

    public async Task<Usuario?> FindByIdAsync(int id) =>
        await _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == id);

    
}