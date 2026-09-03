

namespace SpendWise.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpendWiseDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        private const string NoActiveTransactionMsg = "No active transaction.";

        public UnitOfWork(SpendWiseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction= await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if(_transaction==null)
                throw new InvalidOperationException(NoActiveTransactionMsg);

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction= null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException(NoActiveTransactionMsg);

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync() => 
            await _dbContext.SaveChangesAsync();
        
    }
}
