using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class ImagenProyecto
    {
        [Required]
        public int ImagenID { get; set; }
        [Required]
        public string URL {  get; set; }
        [Required]
        public bool Estado { get; set; }
        [Required]
        public int UsuarioID { get; set; }
        [Required]
        public string PublicID { get; set; }
        [Required]
        public int ProyectoID { get; set; }
        [Required]
        public int Orden {  get; set; }
    }
}
