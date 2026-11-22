using AutoMapper;
using TheTouchdownBarberShop.Application.DTOs;

public class ServiceUsuario : IServiceUsuario
{
    private readonly IRepositoryUsuario _repo;
    private readonly IMapper _mapper;
    public ServiceUsuario(IRepositoryUsuario repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<ICollection<UsuarioDTO>> ListAsync() =>
        _mapper.Map<ICollection<UsuarioDTO>>(await _repo.ListAsync());

    public async Task<UsuarioDTO?> FindByIdAsync(int id)
    {
        var u = await _repo.FindByIdAsync(id);
        return u is null ? null : _mapper.Map<UsuarioDTO>(u);
    }

    public async Task<UsuarioDTO?> ValidarCredencialesAsync(string correo, string contrasenia)
    {
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrEmpty(contrasenia))
            return null;

        var u = await _repo.FindByEmailAsync(correo);
        if (u is null || string.IsNullOrEmpty(u.Contrasenia))
            return null;

        if (!BCrypt.Net.BCrypt.Verify(contrasenia, u.Contrasenia))
            return null;

        return _mapper.Map<UsuarioDTO>(u);
    }
}