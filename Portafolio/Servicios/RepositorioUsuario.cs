using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IRepositorioUsuario
    {
        Task<Usuario> BuscarUsuarioPorEmail(string EmailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
        Task EditarDescripcion(DescripcionUsuario descripcionUsuario);
        Task<DescripcionUsuario> ObtenerDescripcion(int UsuarioID);
        Task<DescripcionUsuario> ObtenerDescripcionVista();
    }

    /// <summary>
    /// Esta clase contiene los metodos necesarios para administrar los usuarios en la base de datos.
    /// </summary>
    public class RepositorioUsuario : IRepositorioUsuario
    {

        private readonly string connectionString;

        public RepositorioUsuario(IDbConnection dbConnection)
        {
            connectionString = dbConnection.GetConnectionString();
        }


        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);

            var parametros = new DynamicParameters();
            parametros.Add("@EmailNormalizado", usuario.EmailNormalizado);
            parametros.Add("@HashContrasena", usuario.HashContrasena);
            parametros.Add("@UsuarioID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("CrearUsuario", parametros, commandType: CommandType.StoredProcedure);
            var id = parametros.Get<int>("@UsuarioID");

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

        public async Task EditarDescripcion(DescripcionUsuario descripcionUsuario)
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"UPDATE DescripcionUsuario 
                        SET Descripcion = @Descripcion
                        WHERE UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, descripcionUsuario);
        }

        public async Task<DescripcionUsuario> ObtenerDescripcion(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"SELECT DescripcionUsuarioID, Descripcion, UsuarioID
                        FROM DescripcionUsuario
                        WHERE UsuarioID = @UsuarioID;";

            var descripcionDeUsuario = await connection.QueryFirstOrDefaultAsync<DescripcionUsuario>(query, new { UsuarioID });

            return descripcionDeUsuario;
        }

        public async Task<DescripcionUsuario> ObtenerDescripcionVista()
        {
            using var connection = new SqlConnection(connectionString);

            var query = @"SELECT DescripcionUsuarioID, Descripcion, UsuarioID
                        FROM DescripcionUsuario";

            var descripcionDeUsuario = await connection.QueryFirstOrDefaultAsync<DescripcionUsuario>(query);

            return descripcionDeUsuario;
        }
    }
}
