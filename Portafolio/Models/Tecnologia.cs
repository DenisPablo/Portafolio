using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    /// <summary>
    /// Esta clase representa una tecnologia usada en un proyecto, podria ser una herramienta por ejemplo NMAP, Docker o un lenguaje de programacion como C# y Python.
    /// </summary>
    public class Tecnologia
    {
        public int TecnologiaID { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El {0} debe tener como minimo {2} y maximo {1} caracteres")]
        [Remote(action: "VerificarExistenciaTecnologia", controller:"Tecnologia")]
        public string Nombre { get; set; }

        [Required]
        public bool Estado { get; set; }
        
        /// <summary>
        /// Cada tecnologia le pertence a un usuario y solo el puede modificar y eliminar la misma. 
        /// </summary>
        [Required]
        public int UsuarioID { get; set; }
    }
}
