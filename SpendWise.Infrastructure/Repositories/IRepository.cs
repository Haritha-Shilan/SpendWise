namespace SpendWise.Infrastructure.Repositories
{
    public interface IRepository<T> where T: class,IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(QueryOptions<T> options);

        Task<T?> GetByIdAsync(int id);

        Task<T?> GetByIdAsync(int id,QueryOptions<T> options);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
