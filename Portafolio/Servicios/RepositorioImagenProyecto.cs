using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;
using System.Data.Common;

namespace Portafolio.Servicios
{

    interface IRepositorioImagenProyecto
    {
        Task<int> Crear(ImagenProyecto imagenProyecto);
        Task EliminarImagenProyecto(int ImagenID, int UsuarioID);
        Task<IEnumerable<ImagenProyecto>> ObtenerImagenesProyecto(int ProyectoID, int UsuarioID);
    }

    public class RepositorioImagenProyecto : IRepositorioImagenProyecto
    {
        private readonly string connectionString;
        public RepositorioImagenProyecto(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Crear(ImagenProyecto imagenProyecto)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"INSERT INTO ImagenProyecto (URL,Estado, Orden, UsuarioID) 
                            VALUES (@URL, 1, @Orden, @UsuarioID);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = await connection.QuerySingleAsync<int>(query, imagenProyecto);
            return id;
        }

        public async Task<IEnumerable<ImagenProyecto>> ObtenerImagenesProyecto(int ProyectoID, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"SELECT ProyectoID, URL
                             FROM ImagenProyecto
                             WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID";

            var imagenesProyecto = await connection.QueryAsync<ImagenProyecto>(query, new { ProyectoID, UsuarioID });
            return imagenesProyecto;
        }

        public async Task EliminarImagenProyecto(int ImagenID, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE ImagenProyecto
                             SET Estado = 0
                             WHERE ImagenID = @ImagenID AND UsuarioID = @UsuarioID";

            await connection.ExecuteAsync(query, new { ImagenID, UsuarioID });
        }
    }
}
