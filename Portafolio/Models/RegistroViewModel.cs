using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string Password2 { get; set; }
    }
}
