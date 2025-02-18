namespace Portafolio.Models
{
    public class ProyectoViewModel
    {
        public int ProyectoID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public IEnumerable<Tecnologia> Tecnologias { get; set;}
        
        public int Antiguedad { get; set; }
        public string Categoria { get; set; }

        public IEnumerable<ImagenProyecto> Imagenes { get; set; }

        public ProyectoViewModel(int proyectoID,string titutlo, string descripcion, IEnumerable<Tecnologia> tecnologias, int antiguedad, string categoria, IEnumerable<ImagenProyecto> imagenes)
        {
            ProyectoID = proyectoID;
            Titulo = titutlo;
            Descripcion = descripcion;
            Tecnologias = tecnologias;
            Antiguedad = antiguedad;
            Categoria = categoria;
            Imagenes = imagenes;
        }
    }
}
