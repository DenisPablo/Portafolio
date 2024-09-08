using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    /// <summary>
    /// Esta clase representa un proyecto base sin especializacion ni herramientas definidas en su uso.
    /// </summary>
    public class Proyecto
    {
        public int ProyectoID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="El titulo es demasiado largo")]
        public string Titulo { get; set; }
        [Required]
        [StringLength(2000, ErrorMessage ="La Descripcion es demasiada extensa. El Maximo son 2000 caracteres")]
        public string Descripcion {  get; set; }
        /// <summary>
        /// Cada proyecto le pertenece a un usuario y solo el puede modificar y eliminar la misma.
        /// </summary>
        [Required]
        public int UsuarioID { get; set; }
        public DateTime FechaPubli {  get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public int CategoriaID { get; set; }

        public int Antiguedad { get; set; }
    }
}
