using Dapper;
using Microsoft.Data.SqlClient;
using PixelGrid_WebApi.Datamodels;


namespace PixelGrid_WebApi.Services
{
    public class SqlEnvironment2DService : ISqlEnvironment2DService
    {
        public static List<Environment2D> Environments = new List<Environment2D>();

        private readonly string _connectionString;

        public SqlEnvironment2DService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertDataAsync(Environment2D data)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO [Environment2D] " +
                    "VALUES (@ID, @Name, @OwnerUserId, @MaxHeight, @MaxLength, @Seed)", data);
                Console.WriteLine("Environment2D object created");
            };
        }

        public async Task UpdateDataAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync(
                   "UPDATE [Environment2D] " +
                   "SET Name = @Name, OwnerUserId = @OwnerUserId, MaxHeight = @MaxHeight, MaxLength = @MaxLength, Seed = @Seed " +
                   "WHERE ID = @ID",
                   environment
               );
            }
        }

        public async Task DeleteDataAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Environment2D] " +
                    "WHERE ID = @id",
                    new { id });
            }
        }

        public async Task<Environment2D> GetDataAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Environment2D>(
                            "SELECT * FROM [Environment2D] " +
                            "WHERE ID = @id", new { id });
            };
        }

        public async Task<IEnumerable<Environment2D>> GetListOfDataAsync(string OwnerUserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE OwnerUserId = @OwnerUserId", new { OwnerUserId });
            };
        }

    }
}
