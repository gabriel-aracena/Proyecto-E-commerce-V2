using Microsoft.EntityFrameworkCore;
using OnlineSpot.Data.Domain.Interfaces;
using OnlineSpot.Data.Persistence.Context;


namespace OnlineSpot.Data.Persistence.Repositories
{
    public class GenericRepository<Entity>(OnlineSpotDbContext context) : IGenericRepository<Entity> where Entity : class
    {

     
        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await context.Set<Entity>().AddAsync(entity);
            await context.SaveChangesAsync();

            return entity;
        }
        public virtual async Task DeleteAsync(Entity entity)
        {
            context.Set<Entity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await context.Set<Entity>().ToListAsync();
        }

        public Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = context.Set<Entity>().AsQueryable();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return query.ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(Guid id)
        {
            return await context.Set<Entity>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(Entity entity, Guid id)
        {
            Entity entry = await context.Set<Entity>().FindAsync(id);
            context.Entry(entry).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
        }


    }
}
