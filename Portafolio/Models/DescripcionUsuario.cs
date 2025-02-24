namespace Portafolio.Models
{
    public class DescripcionUsuario
    {
        public int DescripcionUsuarioID { get;set;}
        public string Descripcion { get; set; }
        public int UsuarioID { get; set; }
    
        public DescripcionUsuario(int descripcionUsuarioID,string descripcion, int usuarioID)
        {
            DescripcionUsuarioID = descripcionUsuarioID;
            Descripcion = descripcion;
            UsuarioID = usuarioID;
        }

        public DescripcionUsuario() { }
    }
}
