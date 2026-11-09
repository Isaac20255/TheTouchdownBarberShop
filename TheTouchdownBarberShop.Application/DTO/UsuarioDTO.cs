namespace TheTouchdownBarberShop.Application.DTOs;

public record UsuarioDTO
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = null!;
    public string NombreRol { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
}