using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IRepositorioTecnologia
    {
        Task<int> Crear(Tecnologia tecnologia);
        Task EditarTecnologia(Tecnologia tecnologia);
        Task EliminarTecnologia(int CategoriaID, int UsuarioID);
        Task<bool> ExisteTecnologia(string nombre, int UsuarioID);
        Task<IEnumerable<Tecnologia>> ObtenerTecnologias(int UsuarioID);
        Task<Tecnologia> ObtenerTecnologiasPorID(int TecnologiaID, int UsuarioID);
    }

    public class RepositorioTecnologia : IRepositorioTecnologia
    {

        private readonly string connectionString;
        public RepositorioTecnologia(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Crea una nueva tecnologia en la base de datos.
        /// </summary>
        /// <param name="tecnologia">instancia de la clase tecnologia con la informacion necesaria</param>
        /// <returns>El idenificador de la tecnologia creada</returns>
        public async Task<int> Crear(Tecnologia tecnologia)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"INSERT INTO Tecnologia (Nombre, UsuarioID, Estado) 
                             VALUES (@Nombre, @UsuarioID, @Estado);
                             SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(query, tecnologia);
            return id;
        }

        /// <summary>
        /// Obtiene todas las categorias de la base de datos que le pertenezcan al usuario activo.
        /// </summary>
        /// <param name="UsuarioID">identificador del usuario</param>
        /// <returns>IEnumerable de tecnologias</returns>
        public async Task<IEnumerable<Tecnologia>> ObtenerTecnologias(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT TecnologiaID, Nombre, UsuarioID, Estado 
                            FROM Tecnologia
                            WHERE UsuarioID = @UsuarioID AND Estado = 1;";

            var tecnologias = await connection.QueryAsync<Tecnologia>(query, new { UsuarioID });

            return tecnologias;
        }

        /// <summary>
        /// Busca una tecnologia por un identificador. NOTA: Debe pertenecer al usuario
        /// </summary>
        /// <param name="TecnologiaID">Identificador de la tecnologia a buscar</param>
        /// <param name="UsuarioID">Identificador del usuario propietario</param>
        /// <returns></returns>
        public async Task<Tecnologia> ObtenerTecnologiasPorID(int TecnologiaID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT TecnologiaID, Nombre, UsuarioID, Estado 
                            FROM Tecnologia 
                            WHERE TecnologiaID = @TecnologiaID AND UsuarioID = @UsuarioID;";

            var tecnologia = await connection.QueryFirstOrDefaultAsync<Tecnologia>(query, new { TecnologiaID, UsuarioID });
            return tecnologia;
        }

        /// <summary>
        /// Marca como deshabilitada una tecnologia (borrado logico)
        /// </summary>
        /// <param name="CategoriaID">identificador de la categoria</param>
        /// <param name="UsuarioID">indentificador del usuario propietario</param>
        /// <returns></returns>
        public async Task EliminarTecnologia(int TecnologiaID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Tecnologia
                            SET Estado = 0
                            WHERE TecnologiaID = @TecnologiaID AND UsuarioID = UsuarioID;";

            await connection.ExecuteAsync(query, new { TecnologiaID, UsuarioID });
        }


        /// <summary>
        /// Permite modificar una tecnologia ya existente.
        /// </summary>
        /// <param name="tecnologia">instancia de la clase tecnologia con la informacion modificada</param>
        /// <returns></returns>
        public async Task EditarTecnologia(Tecnologia tecnologia)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Tecnologia 
                            SET Nombre = @Nombre
                            WHERE TecnologiaID = @TecnologiaID AND UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, tecnologia);  
        }

        public async Task<bool> ExisteTecnologia(string nombre, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT 1 FROM Tecnologia WHERE Nombre = @Nombre AND UsuarioID = @UsuarioID;";

            var existe = await connection.QueryFirstOrDefaultAsync<int>(query, new { nombre, UsuarioID });

            return existe == 1;
        }
    }
}
