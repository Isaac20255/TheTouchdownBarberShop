using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class HorariosBarberos
{
    public int IdHorario { get; set; }

    public int IdBarbero { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Usuarios IdBarberoNavigation { get; set; } = null!;
}
