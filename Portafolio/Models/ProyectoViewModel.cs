namespace Portafolio.Models
{
    public class ProyectoViewModel
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public IEnumerable<Tecnologia> Tecnologias { get; set;}
        
        public int Antiguedad { get; set; }
        public string Categoria { get; set; }

        public ProyectoViewModel(string titutlo, string descripcion, IEnumerable<Tecnologia> tecnologias, int Antiguedad, string categoria)
        {
            Titulo = titutlo;
            Descripcion = descripcion;
            Tecnologias = tecnologias;
            Categoria = categoria;
        }
    }
}
