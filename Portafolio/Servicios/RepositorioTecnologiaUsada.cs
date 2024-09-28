using Dapper;
using Microsoft.Data.SqlClient;

namespace Portafolio.Servicios
{

    public interface IRepositorioTecnologiaUsada
    {
        Task<int> Crear(int ProyectoID, int TecnologiaID, int UsuarioID);
        Task EliminarTecnologia(int TecnologiaID, int UsuarioID);
    }

    public class RepositorioTecnologiaUsada : IRepositorioTecnologiaUsada
    {
        private readonly string connectionString;

        public RepositorioTecnologiaUsada(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Crear(int ProyectoID, int TecnologiaID, int UsuarioID)
        {

            using var connection = new SqlConnection(connectionString);

            string query = @"INSERT INTO TecnologiaUsada (ProyectoID, TecnologiaID, UsuarioID)
                             VALUES (@ProyectoID, @TecnologiaID, @UsuarioID);
                             SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(query, new { ProyectoID, TecnologiaID, UsuarioID });
            return id;
        }

        public async Task EliminarTecnologia(int TecnologiaID, int UsuarioID) {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE TecnologiaUsada
                             SET Estado = 0
                             WHERE TecnologiaID = @TecnologiaID AND UsuarioID = @UsuarioID";

            await connection.ExecuteAsync(query, new { TecnologiaID, UsuarioID });
        }


    }
}
