using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{

    /// <summary>
    /// Representa un usuario que podra administrar y moverse dentro del sistema.
    /// </summary>
    public class Usuario
    {
        public int UsuarioId { get; set; }
        [Required]
        public string HashContrasena { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
