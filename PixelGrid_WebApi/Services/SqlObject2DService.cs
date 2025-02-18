using Dapper;
using Microsoft.Data.SqlClient;
using PixelGrid_WebApi.Datamodels;
using System.Security.Cryptography;

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

        public async Task DeleteDataAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE ID = @id", new { id });
            }
        }

        public async Task<Object2D> GetDataAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE ID = @id", new { id });
            };
        }

        public async Task<IEnumerable<Object2D>> GetListOfDataAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            };
        }
    }
}
