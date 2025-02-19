namespace EindOpdracht.WebApi.Services
{
    public interface IEnvironment2DRepository 
    {
        public Task<IEnumerable<Environment2D>> ReadAsync();
        public Task DeleteAsync(int id);
    }

}
