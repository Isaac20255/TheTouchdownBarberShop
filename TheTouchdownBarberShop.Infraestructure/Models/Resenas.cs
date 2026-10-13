using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Resenas
{
    public int IdResena { get; set; }

    public int IdCita { get; set; }

    public int IdCliente { get; set; }

    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Citas IdCitaNavigation { get; set; } = null!;

    public virtual Usuarios IdClienteNavigation { get; set; } = null!;
}
