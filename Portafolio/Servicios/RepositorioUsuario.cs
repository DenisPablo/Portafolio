using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IRepositorioUsuario
    {
        Task<Usuario> BuscarUsuarioPorEmail(string EmailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
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


        public async Task<int> CrearUsuario(Usuario usuario) 
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"INSERT INTO Usuario (EmailNormalizado,HashContrasena, Estado)
                        VALUES (@EmailNormalizado, @HashContrasena, 1);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = await connection.QuerySingleAsync<int>(query, usuario);

            return id;
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string EmailNormalizado) 
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"SELECT UsuarioID ,EmailNormalizado ,HashContrasena ,Estado From Usuario
                        WHERE EmailNormalizado = @EmailNormalizado AND Estado = 1;";

            Usuario usuario = await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { EmailNormalizado });

            return usuario;
        }
    }
}
