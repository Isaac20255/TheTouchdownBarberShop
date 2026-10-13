using System;
using System.Collections.Generic;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class CitaPreguntas
{
    public int IdCitaPregunta { get; set; }

    public int IdCita { get; set; }

    public string Pregunta { get; set; } = null!;

    public string Respuesta { get; set; } = null!;

    public virtual Citas IdCitaNavigation { get; set; } = null!;
}
