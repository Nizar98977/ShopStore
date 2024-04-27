using Core.Entites;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        private readonly ILogger _logger;
        public GenericRepository(StoreContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(e => e.Id == id);
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> sortBy, bool isDescending, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (sortBy != null)
            {
                if (isDescending)
                {
                    query = query.OrderByDescending(sortBy);
                }
                else
                {
                    query = query.OrderBy(sortBy);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                EntityEntry entityEntry = _context.Entry<T>(entity);
                entityEntry.State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogError($"Product with ID {id} not found");
                throw new InvalidOperationException($"Product with ID {id} not found");
            }
        }
    }
}

