using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheTouchdownBarberShop.Infraestructure.Models;

public partial class Sucursal
{
    public int IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int? AdministradorId { get; set; }
  
    public virtual Usuario? Administrador { get; set; }

    public virtual ICollection<HorariosSucursal> HorariosSucursals { get; set; } = new List<HorariosSucursal>();


}