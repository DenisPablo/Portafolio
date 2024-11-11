using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{

    public interface IRepositorioTecnologiaUsada
    {
        Task<int> Crear(int ProyectoID, int TecnologiaID, int UsuarioID);
        Task EliminarTecnologia(int TecnologiaID, int UsuarioID);
        Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyecto(int ProyectoID, int UsuarioID);
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

        public async Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyecto(int ProyectoID, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT t.TecnologiaID, t.Nombre, t.Estado, t.UsuarioID From Tecnologia t 
                            JOIN TecnologiaUsada tu ON t.TecnologiaID = tu.TecnologiaID
                            WHERE tu.ProyectoID = @ProyectoID AND t.UsuarioID = @UsuarioID AND tu.UsuarioID = @UsuarioID;";
            
            var tecnologias = await connection.QueryAsync<Tecnologia>(query, new { ProyectoID, UsuarioID });
            return tecnologias;
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
