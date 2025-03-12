namespace EindOpdracht.WebApi.Services
{
    public interface IEnvironment2DRepository 
    {
        public Task DeleteAsync(Guid id);
        public Task<IEnumerable<Environment2D>> ReadAsync();
        public Task<IEnumerable<Environment2D?>> ReadWorldsFromUserAsync(string ownerUserId);
        public Task<Environment2D?> ReadAsync(Guid id);
        public Task<IEnumerable<Object2D>> ReadObjectsByEnvironment(Guid environmentId);
        public Task<Environment2D> InsertAsync(Environment2D environment2D);
        public Task UpdateAsync(Environment2D environment);



    }

}
