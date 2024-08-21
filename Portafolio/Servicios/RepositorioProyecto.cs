using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;

namespace Portafolio.Servicios
{
    public interface IRepositorioProyecto
    {
        Task<int> Crear(Proyecto proyecto);
        Task EditarProyecto(Proyecto proyecto);
        Task EliminarProyecto(int ProyectoID, int UsuarioID);
        Task<bool> ExisteProyecto(string nombre, int UsuarioID);
        Task<Proyecto> ObtenerProyectoPorID(int ProyectoID, int UsuarioID);
        Task<IEnumerable<Proyecto>> ObtenerProyectos(int usuarioID);
    }

    /// <summary>
    /// Esta clase contiene los metodos necesarios para administrar los Proyectos en la base de datos
    /// </summary>

    public class RepositorioProyecto : IRepositorioProyecto {

        private readonly string connectionString;

        public RepositorioProyecto(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Se conecta a la base de datos y crea un nuevo Proyecto
        /// </summary>
        /// <param name="proyecto">El proyecto con la informacion a crear</param>
        /// <returns>El id del proyecto creado</returns>
        public async Task<int> Crear(Proyecto proyecto)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"INSERT INTO Proyecto (Titulo, Descripcion, Estado, UsuarioID) 
                            VALUES (@Titulo, @Descripcion, 1, @UsuarioID);
                            SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync(query, proyecto);
            return id;
        }


        /// <summary>
        /// Obtiene los proyectos de un usuario de la base de datos.
        /// </summary>
        /// <param name="usuarioID">Identifica al propietario de los proyectos</param>
        /// <returns>Enumerable de proyectos</returns>
        public async Task<IEnumerable<Proyecto>> ObtenerProyectos(int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad
                            FROM Proyecto 
                            WHERE UsuarioID = @UsuarioID AND Estado = 1;";

            var proyectos = await connection.QueryAsync<Proyecto>(query, new { UsuarioID });
            
            return proyectos;
        }

        /// <summary>
        /// Busca un proyecto en específico por su ID.
        /// </summary>
        /// <param name="CategoriaID">Identificador del proyecto a buscar.</param>
        /// <param name="UsuarioID">Identificador del propietario del proyecto.</param>
        /// <returns>El proyecto encontrado o null si no existe.</returns>
        public async Task<Proyecto> ObtenerProyectoPorID(int ProyectoID, int UsuarioID) 
        {
            using var connecion = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado 
                            FROM Proyecto 
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID AND Estado = 1;";

            var proyecto = await connecion.QueryFirstOrDefaultAsync(query, new { ProyectoID, UsuarioID });
            return proyecto;
        }

        /// <summary>
        /// Marca un proyecto como deshabilitado (borrado lógico).
        /// </summary>
        /// <param name="ProyectoID">Identifica el proyecto a deshabilitar.</param>
        /// <param name="UsuarioID">Identifica al propietario del proyecto.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task EliminarProyecto(int ProyectoID, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Proyecto 
                            SET Estado = 0 
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID AND Estado = 1;";

            await connection.ExecuteAsync(query, new { ProyectoID, UsuarioID });

        }

        /// <summary>
        /// Permite realizar ediciones sobre los proyectos existentes.
        /// </summary>
        /// <param name="proyecto">Instancia de un proyecto con la nueva información a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task EditarProyecto(Proyecto proyecto) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Proyecto 
                            SET Titulo = 'test1', Descripcion = 'testdescrip2' 
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID AND Estado = 1;";

            await connection.ExecuteAsync(query, proyecto);
        }

        /// <summary>
        /// Verifica si un proyecto con un nombre específico ya existe para un usuario.
        /// </summary>
        /// <param name="nombre">Nombre del proyecto a buscar.</param>
        /// <param name="UsuarioID">Identificador del propietario del proyecto.</param>
        /// <returns>Un valor booleano que indica si existe o no el proyecto.</returns>
        public async Task<bool> ExisteProyecto(string nombre, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT 1 
                            FROM Proyecto 
                            WHERE Titulo = @Titulo AND UsuarioID = @UsuarioID";

            var existe = await connection.QueryFirstOrDefaultAsync<int>(query, new { nombre, UsuarioID });

            return existe == 1;
        }
    }
}
