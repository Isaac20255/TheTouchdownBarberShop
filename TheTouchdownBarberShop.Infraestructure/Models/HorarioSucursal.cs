namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class HorariosSucursal
{
    public int IdHorarioSucursal { get; set; }

    public int IdSucursal { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Sucursal IdSucursalNavigation { get; set; } = null!;
}