namespace Portafolio.Servicios
{
    public interface IRepositorioUsuario
    {
        Task<int> ObtenerUsuario();
    }

    /// <summary>
    /// Esta clase contiene los metodos necesarios para administrar los usuarios en la base de datos.
    /// </summary>
    public class RepositorioUsuario : IRepositorioUsuario
    {

        private readonly string connectionString;

        public RepositorioUsuario(IConfiguration configuration) {

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Task<int> ObtenerUsuario() 
        {
            return Task.FromResult(1);
        }
    }
}
