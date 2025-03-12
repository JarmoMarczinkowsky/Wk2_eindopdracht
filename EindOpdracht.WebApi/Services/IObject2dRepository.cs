namespace EindOpdracht.WebApi.Services
{
    public interface IObject2dRepository
    {
        public Task<IEnumerable<Object2D>> ReadAsync();
        public Task<Object2D?> ReadAsync(Guid id);
        public Task<Object2D> InsertAsync(Object2D object2D);
        public Task UpdateAsync(Object2D object2D);
        public Task DeleteAsync(Guid id);
    }
}
