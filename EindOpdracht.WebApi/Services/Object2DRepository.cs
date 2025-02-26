using Dapper;
using Microsoft.Data.SqlClient;

namespace EindOpdracht.WebApi.Services
{
    public class Object2DRepository
    {
        public static List<Object2D> lstObject2d = new List<Object2D>();

        private string _connstr;

        public Object2DRepository(string connectionString)
        {
            _connstr = connectionString;
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task<Object2D?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadByEnvironmentIdAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D] WHERE EnvironmentID = @EnvironmentID", new { environmentId });
            }
        }

        public async Task<Object2D> InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                var objectId = await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentID) VALUES (@PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentID)", object2D);
                return object2D;
            }
        }

        public async Task UpdateAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(_connstr))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "PrefabId = @PrefabId, " +
                                                 "PositionX = @PositionX, " +
                                                 "PositionY = @PositionY," +
                                                 "scaleX = @scaleX," +
                                                 "scaleY = @scaleY," +
                                                 "rotationZ = @rotationZ," +
                                                 "sortingLayer = @sortingLayer," +
                                                 "Where Id = @Id"
                                                 , object2D);

            }
        }

    }
}
