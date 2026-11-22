using System.ComponentModel.DataAnnotations;

namespace TheTouchdownBarberShop.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get; set; } = string.Empty;
}