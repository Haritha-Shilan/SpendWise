namespace SpendWise.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();

        public Task BeginTransactionAsync();

        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();


    }
}
