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
}