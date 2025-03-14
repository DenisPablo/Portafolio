using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    /// <summary>
    /// Esta clase representa un proyecto base sin especializacion ni herramientas definidas en su uso.
    /// </summary>
    public class Proyecto
    {
        public int ProyectoID { get; set; }
        [Required(ErrorMessage = "El titulo es obligatorio")]
        [Remote(action: "VerificarExistenciaProyecto", controller: "Proyecto", AdditionalFields = "ProyectoID", ErrorMessage = "El título ya está en uso.")]
        [StringLength(50, ErrorMessage = "El titulo es demasiado largo")]
        public string Titulo { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int UsuarioID { get; set; }
        public DateTime FechaPubli { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public int CategoriaID { get; set; }

        public int Antiguedad { get; set; }

        public string UrlGitHub { get; set; }
        public string UrlDesplegado { get; set; }
    }
}
