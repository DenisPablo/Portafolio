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
        Task<IEnumerable<Categoria>> ObtenerCategorias(int id);
        Task<Categoria> ObtenerCategoriasPorID(int CategoriaID, int UsuarioID);
    }

    public class RepositorioCategoria : IRepositorioCategoria
    {
        private readonly string connectionString;

        public RepositorioCategoria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"INSERT INTO Categoria (Nombre,Estado,UsuarioID) 
                            VALUES (@Nombre, @Estado, @UsuarioID);
                            SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(query, categoria);
            return id;
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategorias(int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"SELECT CategoriaID, Nombre, Estado 
                             FROM Categoria 
                             WHERE UsuarioID = @UsuarioID AND Estado = 1";

            var categorias = await connection.QueryAsync<Categoria>(query, new { UsuarioID });
            return categorias;
        }

        public async Task<Categoria>ObtenerCategoriasPorID(int CategoriaID ,int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"SELECT CategoriaID, Nombre, Estado 
                             FROM Categoria 
                             WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID AND Estado = 1";

            var categorias = await connection.QueryFirstOrDefaultAsync<Categoria>(query, new { CategoriaID, UsuarioID });
            return categorias;
        }

        public async Task EliminarCategoria(int CategoriaID, int UsuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            string query = @"UPDATE Categoria 
                            SET Estado = 0 
                            WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, new { CategoriaID, UsuarioID });
        }


        public async Task EditarCategoria(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);

            string query = @"UPDATE Categoria 
                            SET Nombre = @Nombre 
                            WHERE CategoriaID = @CategoriaID AND UsuarioID = @UsuarioID;";

            await connection.ExecuteAsync(query, categoria);
        }
    }
}