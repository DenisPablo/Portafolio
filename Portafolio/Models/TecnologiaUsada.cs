using System.ComponentModel.DataAnnotations;

namespace Portafolio.Models
{
    public class TecnologiaUsada
    {
       [Required]
       public int TecnologiaUsadaID { set; get; }
       [Required]
       public int ProyectoID { set; get; }
       [Required]
       public int TecnologiaID { set; get; }
       [Required]
       public int UsuarioID { set; get; }
    }
}
