using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class Proyecto
    {
        public int ProyectoID { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        [StringLength(2000, ErrorMessage ="La Descripcion es demasiada extensa. El Maximo son 2000 caracteres")]
        public string Descripcion {  get; set; }
        [Required]
        public int UsuarioID { get; set; }
        [Required]
        public int Categoria { get; set; }
    }
}
