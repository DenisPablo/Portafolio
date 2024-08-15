using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        [Required]
        public string HashContrasena { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
