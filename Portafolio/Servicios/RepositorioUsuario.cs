namespace Portafolio.Servicios
{
    public interface IRepositorioUsuario
    {
        Task<int> ObtenerUsuario();
    }
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
