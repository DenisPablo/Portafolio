using Dapper;
using Microsoft.Data.SqlClient;
using Portafolio.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Portafolio.Servicios
{
    public interface IRepositorioCategoria
    {
        Task<int> Crear(Categoria categoria);
        Task EditarCategoria(Categoria categoria);
        Task EliminarCategoria(int CategoriaID, int UsuarioID);
        Task<bool> ExisteCategoria(string nombre, int UsuarioID);
        Task<IEnumerable<Categoria>> ObtenerCategorias(int id);
        Task<Categoria> ObtenerCategoriasPorID(int CategoriaID, int UsuarioID);
    }

    /// <summary>
    /// Esta clase repositorio contiene los metodos necesarios para administrar un categoria interactuando con la base de datos.
    /// </summary>

    public class RepositorioCategoria : IRepositorioCategoria
    {
        private readonly string connectionString;

        public RepositorioCategoria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Se conecta con la base de datos y crea una nueva categoria.
        /// </summary>
        /// <param name="categoria">Instancia de una clase categoria con la informacion necesaria para cargarla</param>
        /// <returns>retorna el identificador de la categoria creada</returns>
        public async Task<int> Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"INSERT INTO Categoria (Nombre,Estado,UsuarioID) 
                            VALUES (@Nombre, 1, @UsuarioID);
                            SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(query, categoria);
            return id;
        }

        /// <summary>
        /// Obtiene todas las categorias de la base datos que le pertenecen a un usuario.
        /// </summary>
        /// <param name="UsuarioID">Identifica al propietario de las categorias</param>
        /// <returns>IEnumerable de las categorias obtenidas</returns>
        public async Task<IEnumerable<Categoria>> ObtenerCategorias(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"SELECT CategoriaID, Nombre, Estado 
                             FROM Categoria 
                             WHERE UsuarioID = @UsuarioID AND Estado = 1";

            var categorias = await connection.QueryAsync<Categoria>(query, new { UsuarioID });
            return categorias;
        }

        /// <summary>
        /// Busca una categoria en especifico identificandola con el ID.
        /// </summary>
        /// <param name="CategoriaID">identificador de la categoria a buscar</param>
        /// <param name="UsuarioID">identificador del propietario de la categoria</param>
        /// <returns></returns>
        public async Task<Categoria>ObtenerCategoriasPorID(int CategoriaID ,int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"SELECT CategoriaID, Nombre, Estado 
                             FROM Categoria 
                             WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID AND Estado = 1";

            var categoria = await connection.QueryFirstOrDefaultAsync<Categoria>(query, new { CategoriaID, UsuarioID });
            return categoria;
        }


        /// <summary>
        /// Marca una categoria como deshabilitada (borrado logico).
        /// </summary>
        /// <param name="CategoriaID">identifica la categoria a deshabilitar</param>
        /// <param name="UsuarioID">identifica al propietario de la categoria</param>
        /// <returns></returns>
        public async Task EliminarCategoria(int CategoriaID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"UPDATE Categoria 
                            SET Estado = 0 
                            WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID AND Estado = 1;";

            await connection.ExecuteAsync(query, new { CategoriaID, UsuarioID });
        }

        /// <summary>
        /// Permite realizar ediciones sobre las categorias ya existentes.
        /// </summary>
        /// <param name="categoria">instancia de una categoria con la nueva informacion a cargar</param>
        /// <returns></returns>
        public async Task EditarCategoria(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Categoria 
                            SET Nombre = @Nombre 
                            WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID AND Estado = 1;";

            await connection.ExecuteAsync(query, categoria);
        }

        /// <summary>
        /// Valida si existe una categoria buscado por nombre, interactua con una validacion en el frontend [Remote]
        /// </summary>
        /// <param name="nombre">Nombre de la categoria</param>
        /// <param name="UsuarioID">Identificador del Usuario</param>
        /// <returns>bool en true si encuentra coincidencias</returns>
        public async Task<bool> ExisteCategoria(string nombre, int UsuarioID) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"SELECT 1 
F                          FROM Categoria
                           WHERE Nombre = @Nombre AND UsuarioID = @UsuarioID;";

            var existe = await connection.QueryFirstOrDefaultAsync<int>(query, new { nombre, UsuarioID });

            return existe == 1;
        }
    }
}