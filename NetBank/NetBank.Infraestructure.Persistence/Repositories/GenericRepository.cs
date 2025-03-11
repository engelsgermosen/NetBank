using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Infraestructure.Persistence.Context;

namespace NetBank.Infraestructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<Entity>> AddRangeAsync(List<Entity> entities)
        {
            await _context.Set<Entity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task Delete(Entity entity)
        {
            _context.Set<Entity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _context.Set<Entity>().ToListAsync();
        }

        public virtual async Task<List<Entity>> GetAllWithIncludesAsync(List<string> includes)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GetById(int id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }

        public virtual IQueryable<Entity> GetQuery()
        {
            return _context.Set<Entity>().AsQueryable();
        }

        public virtual IQueryable<Entity> GetQueryWithIncludes(List<string> includes)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (string include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public virtual async Task<Entity> UpdateAsync(Entity entity, int id)
        {
            Entity? entidad = await _context.Set<Entity>().FindAsync(id);
            _context.Set<Entity>().Entry(entidad).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
