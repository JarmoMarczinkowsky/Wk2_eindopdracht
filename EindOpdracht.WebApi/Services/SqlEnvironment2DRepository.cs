using Dapper;
using Microsoft.Data.SqlClient;

namespace EindOpdracht.WebApi.Services
{
    public class SqlEnvironment2DRepository : IEnvironment2DRepository
    {
        public static List<Environment2D> lstEnvironment2D = new List<Environment2D>();
        
        private string _connstr;

        public SqlEnvironment2DRepository(string connectionString)
        {
            _connstr = connectionString;
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D]");
            }
        }

        public async Task<IEnumerable<Environment2D?>> ReadWorldsFromUserAsync(string ownerUserId)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE OwnerUserId = @OwnerUserId", new { ownerUserId });
            }
        }

        public async Task<Environment2D?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<Environment2D> InsertAsync(Environment2D environment2D)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                //Guid newId = Guid.NewGuid();
                //environment2D.Id = newId;
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, Name, OwnerUserId, MaxHeight, MaxLength) VALUES (@Id, @Name, @OwnerUserId, @MaxHeight, @MaxLength)", environment2D);
                return environment2D;
            }
        }

        public async Task UpdateAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Environment2D] SET " +
                                                 "Name = @Name, " +
                                                 "MaxHeight = @MaxHeight, " +
                                                 "MaxLength = @MaxLength " + 
                                                 "Where Id = @Id"
                                                 , environment);

            }
        }

    }
}
