using Dapper;
using Microsoft.Data.SqlClient;
using PixelGrid_WebApi.Datamodels;

namespace PixelGrid_WebApi.Services
{
    public class SqlObject2DService : ISqlObject2DService
    {
        public static List<Object2D> Environments = new List<Object2D>();

        private readonly string _connectionString;

        public SqlObject2DService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertDataAsync(Object2D obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Explicitly open connection
                await connection.ExecuteAsync(
                    @"INSERT INTO Object2D (ID, EnvironmentID, PrefabID, PosX, PosY, ScaleX, ScaleY, RotationZ, SortingLayer) 
              VALUES (@ID, @EnvironmentID, @PrefabID, @PosX, @PosY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer);",
                    obj
                );
            }
        }


        public async Task UpdateDataAsync(Object2D obj)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Object2D] " +
                    "SET PrefabID = @PrefabID, PosX = @PosX, PosY = @PosY, " +
                    "ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer " +
                    "WHERE ID = @ID",
                    obj
                );
            }
        }

        public async Task DeleteDataAsync(Guid environmentID, Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE ID = @id AND EnvironmentID = @environmentID", new { id, environmentID });
            }
        }


        /// <summary>
        /// Get all objects in with the Guid 'environmentID' in the database
        /// </summary>
        /// <param name="environmentID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Object2D>> GetDataAsync(Guid environmentID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Object2D>(
                    "SELECT * FROM [Object2D] WHERE EnvironmentID = @environmentID",
                    new { environmentID });
            }
        }
    }
}
