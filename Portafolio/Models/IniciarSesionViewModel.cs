using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class IniciarSesionViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        public bool Recordar { get; set; }
    }
}
