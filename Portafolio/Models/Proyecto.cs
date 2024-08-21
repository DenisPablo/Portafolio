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
        /// 
        [Required]
        public int UsuarioID { get; set; }
        [Required]
        public DateTime FechaPubli {  get; set; }

        /// <summary>
        /// Es la antiguedad del proyecto en meses o años que tiene un proyecto y es calculada por la base de datos.
        /// </summary>
        [Required]
        public int Antiguedad { get; set; }
    }
}
