using SpendWise.Infrastructure.Data;

namespace SpendWise.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly SpendWiseDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public Repository(SpendWiseDbContext dbContext)
        {
         _dbContext = dbContext;
         _dbSet= dbContext.Set<T>();
            
        }
        public async Task AddAsync(T entity)
        {
           await  _dbSet.AddAsync(entity);
           await  _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity =await _dbSet.FindAsync(id);
            if (entity == null)
                return;

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()=>await _dbSet.ToListAsync();


        public async Task<IEnumerable<T>> GetAllAsync(QueryOptions<T> options)
        {
            IQueryable <T> query= _dbSet;
            query= ApplyQueryOptions(query, options);
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)=> await _dbSet.FindAsync(id);

        public async Task<T?> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query= _dbSet;

            query = ApplyQueryOptions(query, options);

            var key = _dbContext.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (key == null)
            {
                throw new InvalidOperationException(
                    $"Entity {typeof(T).Name} does not have a primary key.");
            }

            var primaryKey = key?.Name;

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKey!) == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> ApplyQueryOptions(IQueryable<T> query,QueryOptions<T> options)
        {
            if(options.HasFilter)
            {
                foreach(var filter in options.Filters)
                {
                  query=  query.Where(filter);
                }
            }

            if(options.HasOrderBy)
            {
                query = options.OrderDescending ? 
                    query.OrderByDescending(options.OrderBy!) : query.OrderBy(options.OrderBy!);
            }

            foreach(var include in options.GetIncludes)
            {
                query=query.Include(include);
            }

            return query;
        }
    }
}
