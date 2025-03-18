using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;
using System.Data;

namespace Portafolio.Servicios
{

    public interface IRepositorioTecnologiaUsada
    {
        Task<int> Crear(int ProyectoID, int TecnologiaID, int UsuarioID);
        Task EliminarTecnologia(int TecnologiaID, int ProyectoID, int UsuarioID);
        Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyecto(int ProyectoID, int UsuarioID);
        Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyectoDetalles(int ProyectoID);
    }

    public class RepositorioTecnologiaUsada : IRepositorioTecnologiaUsada
    {
        private readonly string connectionString;

        public RepositorioTecnologiaUsada(IDbConnection dbConnection)
        {
            connectionString = dbConnection.GetConnectionString();
        }
        /// <summary>
        /// Crea una asociacion entre un proyecto y una tecnologia en la base de datos.
        /// </summary>
        /// <param name="ProyectoID"></param>
        /// <param name="TecnologiaID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        public async Task<int> Crear(int ProyectoID, int TecnologiaID, int UsuarioID)
        {

            using var connection = new SqlConnection(connectionString);

            string query = @"INSERT INTO TecnologiaUsada (ProyectoID, TecnologiaID, UsuarioID)
                             VALUES (@ProyectoID, @TecnologiaID, @UsuarioID);
                             SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(query, new { ProyectoID, TecnologiaID, UsuarioID });
            return id;
        }
        /// <summary>
        /// Devuelve todos las tecnologias asociadas a un proyecto
        /// </summary>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyecto(int ProyectoID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT t.TecnologiaID, t.Nombre, t.URLIcon, t.Estado, t.UsuarioID 
                            From Tecnologia t 
                            JOIN TecnologiaUsada tu ON t.TecnologiaID = tu.TecnologiaID
                            WHERE tu.ProyectoID = @ProyectoID AND t.UsuarioID = @UsuarioID AND tu.UsuarioID = @UsuarioID;";

            var tecnologias = await connection.QueryAsync<Tecnologia>(query, new { ProyectoID, UsuarioID });
            return tecnologias;
        }
        /// <summary>
        /// Obtiene todas las tecnologias relacionadas a un proyecto
        /// </summary>
        /// <param name="ProyectoID"></param>
        /// <returns>IEnumerable de Tecnologias</returns>
        public async Task<IEnumerable<Tecnologia>> ObtenerTecnologiasProyectoDetalles(int ProyectoID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT t.TecnologiaID, t.Nombre, t.URLIcon, t.Estado, t.UsuarioID
							From Tecnologia t 
                            JOIN TecnologiaUsada tu ON t.TecnologiaID = tu.TecnologiaID
                            WHERE tu.ProyectoID = @ProyectoID;";

            var tecnologias = await connection.QueryAsync<Tecnologia>(query, new { ProyectoID });
            return tecnologias;
        }
        /// <summary>
        /// Borra la asociacion entre una tecnologia y un proyecto
        /// </summary>
        /// <param name="TecnologiaID"></param>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        public async Task EliminarTecnologia(int TecnologiaID, int ProyectoID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            var parametros = new { TecnologiaID, ProyectoID, UsuarioID };

            await connection.ExecuteAsync("EliminarTecnologiaUsada", parametros, commandType: CommandType.StoredProcedure);
        }
    }
}
