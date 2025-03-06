namespace NetBank.Core.Application.Services.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);

        Task<List<Entity>> AddRangeAsync(List<Entity> entities);

        Task<Entity> UpdateAsync(Entity entity, int id);

        Task<Entity> GetById(int id);

        Task<List<Entity>> GetAllAsync();
        Task<List<Entity>> GetAllWithIncludesAsync(List<string> includes);
        IQueryable<Entity> GetQuery();
        IQueryable<Entity> GetQueryWithIncludes(List<string> includes);

        Task Delete(Entity entity);
    }
}
