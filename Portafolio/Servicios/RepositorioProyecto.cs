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
        Task<bool> ExisteProyecto(string Titulo, int ProyectoID, int UsuarioID);
        Task<Proyecto> ObtenerProyectoPorID(int ProyectoID, int UsuarioID);
        Task<IEnumerable<Proyecto>> ObtenerProyectosActivos(int usuarioID);
        Task<Proyecto> ObtenerProyectoDetalle(int ProyectoID);
        Task<IEnumerable<Proyecto>> ObtenerProyectosVisitante();
        Task<IEnumerable<Proyecto>> ObtenerUltimosProyectos();
        Task<IEnumerable<Proyecto>> ObtenerProyectosInactivos(int UsuarioID);
        Task RestaurarProyecto(int ProyectoID, int UsuarioID);
    }

    public class RepositorioProyecto : IRepositorioProyecto
    {

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

            string query = @"INSERT INTO Proyecto (Titulo, Descripcion, FechaPubli, CategoriaID, Estado, UsuarioID, UrlGitHub, UrlDesplegado) 
                     VALUES (@Titulo, @Descripcion, @FechaPubli, @CategoriaID, 1, @UsuarioID, @UrlGitHub, @UrlDesplegado);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = await connection.QuerySingleAsync<int>(query, proyecto);
            return id;
        }
        /// <summary>
        /// Obtiene los proyectos activos de un usuario de la base de datos.
        /// </summary>
        /// <param name="usuarioID">Identifica al propietario de los proyectos</param>
        /// <returns>Enumerable de proyectos</returns>
        public async Task<IEnumerable<Proyecto>> ObtenerProyectosActivos(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado, CategoriaID, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub,             UrlDesplegado
                            FROM Proyecto 
                            WHERE UsuarioID = @UsuarioID AND Estado = 1
                            ORDER BY FechaPubli DESC;";

            var proyectos = await connection.QueryAsync<Proyecto>(query, new { UsuarioID });

            return proyectos;
        }
        /// <summary>
        /// Obtiene los proyectos inactivos un usuario de la base de datos.
        /// </summary>
        /// <param name="usuarioID">Identifica al propietario de los proyectos</param>
        /// <returns>Enumerable de proyectos</returns>
        public async Task<IEnumerable<Proyecto>> ObtenerProyectosInactivos(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado, CategoriaID, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub, UrlDesplegado
                            FROM Proyecto 
                            WHERE UsuarioID = @UsuarioID AND Estado = 0
                            ORDER BY FechaPubli DESC;";

            var proyectos = await connection.QueryAsync<Proyecto>(query, new { UsuarioID });

            return proyectos;
        }
        /// <summary>
        /// Obtiene los detalles de un proyecto
        /// </summary>
        /// <returns>IEnumerable proyectos</returns>
        public async Task<Proyecto> ObtenerProyectoDetalle(int ProyectoID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado, CategoriaID, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub, UrlDesplegado
                            FROM Proyecto 
                            WHERE ProyectoID = @ProyectoID AND Estado = 1;";

            var proyectos = await connection.QueryFirstOrDefaultAsync<Proyecto>(query, new { ProyectoID });

            return proyectos;
        }
        /// <summary>
        /// Obtiene todos los proyectos en la base de datos.
        /// </summary>
        /// <returns>Enumerable de proyectos</returns>
        public async Task<IEnumerable<Proyecto>> ObtenerProyectosVisitante()
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, Estado, CategoriaID, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub, UrlDesplegado
                            FROM Proyecto 
                            WHERE Estado = 1
                            ORDER BY FechaPubli DESC;";

            var proyectos = await connection.QueryAsync<Proyecto>(query);

            return proyectos;
        }
        /// <summary>
        /// Busca un proyecto en específico por su ID.
        /// </summary>
        /// <param name="ProyectoID">Identificador del proyecto a buscar.</param>
        /// <param name="UsuarioID">Identificador del propietario del proyecto.</param>
        /// <returns>El proyecto encontrado o null si no existe.</returns>
        public async Task<Proyecto> ObtenerProyectoPorID(int ProyectoID, int UsuarioID)
        {
            using var connecion = new SqlConnection(connectionString);

            string query = @"SELECT ProyectoID, Titulo, FechaPubli, Descripcion,CategoriaID, UsuarioID, Estado, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub, UrlDesplegado 
                            FROM Proyecto 
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID;";

            var proyecto = await connecion.QueryFirstOrDefaultAsync<Proyecto>(query, new { ProyectoID, UsuarioID });
            return proyecto;
        }

        /// <summary>
        /// Marca un proyecto como deshabilitado (borrado lógico).
        /// </summary>
        /// <param name="ProyectoID">Identifica el proyecto a deshabilitar.</param>
        /// <param name="UsuarioID">Identifica al propietario del proyecto.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task RestaurarProyecto(int ProyectoID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Proyecto 
                            SET Estado = 1
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, new { ProyectoID, UsuarioID });

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
                            SET Titulo = @Titulo, Descripcion = @Descripcion, CategoriaID = @CategoriaID, UrlGitHub = @UrlGitHub, UrlDesplegado = @UrlDesplegado
                            WHERE ProyectoID = @ProyectoID AND UsuarioID = @UsuarioID AND Estado = 1;";

            await connection.ExecuteAsync(query, proyecto);
        }
        /// <summary>
        /// Verifica si un proyecto con un nombre específico ya existe para un usuario.
        /// </summary>
        /// <param name="nombre">Nombre del proyecto a buscar.</param>
        /// <param name="UsuarioID">Identificador del propietario del proyecto.</param>
        /// <returns>Un valor booleano que indica si existe o no el proyecto.</returns>
        public async Task<bool> ExisteProyecto(string Titulo, int ProyectoID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT 1 
                            FROM Proyecto 
                            WHERE Titulo = @Titulo AND ProyectoID <> @ProyectoID AND UsuarioID = @UsuarioID";

            var existe = await connection.QueryFirstOrDefaultAsync<int>(query, new { Titulo, ProyectoID, UsuarioID });

            return existe == 1;
        }
        /// <summary>
        /// Obtiene los ultimos 3 proyectos de la base de datos
        /// </summary>
        /// <returns>IEnumerable de Proyectos</returns>
        public async Task<IEnumerable<Proyecto>> ObtenerUltimosProyectos()
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT TOP (3) ProyectoID, Titulo, FechaPubli, Descripcion, UsuarioID, CategoriaID, Estado, DATEDIFF(MONTH, FechaPubli, GETDATE()) as Antiguedad, UrlGitHub, UrlDesplegado
                            FROM Proyecto
                            WHERE Estado = 1
                            ORDER BY FechaPubli DESC;";


            var proyectos = await connection.QueryAsync<Proyecto>(query);

            return proyectos;
        }
    }
}
