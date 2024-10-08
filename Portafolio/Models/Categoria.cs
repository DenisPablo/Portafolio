﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    /// <summary>
    /// Esta clase representa una categoria de un proyecto de desarrollo o hacking.
    /// </summary>
    public class Categoria
    {
        public int CategoriaID { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El {0} debe tener como minimo {2} y maximo {1} caracteres")]
        [Remote(action: "VerificarExistenciaCategoria", controller:"Categoria")]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
        /// <summary>
        /// Cada categoria le pertenece a un usuario y solo el podra modificar y eliminar la misma.
        /// </summary>
        [Required]
        public int UsuarioID { get; set; }

    }
}
