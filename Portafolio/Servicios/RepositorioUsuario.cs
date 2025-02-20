using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IRepositorioUsuario
    {
        Task<int> AñadirDescrípcion(DescripcionUsuario descripcionUsuario);
        Task<Usuario> BuscarUsuarioPorEmail(string EmailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
        Task EditarDescripcion(DescripcionUsuario descripcionUsuario);
        Task<DescripcionUsuario> ObtenerDescripcion(int UsuarioID);
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

        public async Task<int> AñadirDescrípcion(DescripcionUsuario descripcionUsuario) 
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"INSERT INTO DescripcionUsuario (UsuarioID, Descripcion) VALUES (@UsuarioID, @Descripcion);
                          SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = await connection.QuerySingleAsync<int>(query, descripcionUsuario);

            return id;
        }

        public async Task EditarDescripcion(DescripcionUsuario descripcionUsuario) 
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"UPDATE DescripcionUsuario 
                        SET Descripcion = @DescripcionUsuario
                        WHERE DescripcionUsuarioID = @DescripcionUsuarioID AND UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, descripcionUsuario);
        }

        public async Task<DescripcionUsuario> ObtenerDescripcion(int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"SELECT DescripcionUsuarioID, Descripcion, UsuarioID
                          WHERE UsuarioID = @UsuarioID;";

            var descripcionDeUsuario = await connection.QueryFirstOrDefaultAsync<DescripcionUsuario>(query, new { UsuarioID });

            return descripcionDeUsuario;
        }

    }
}
