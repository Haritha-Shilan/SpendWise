using TransactionEntity = SpendWise.Domain.Entities.Transaction;
namespace SpendWise.API.Features.Transaction
{
    public interface ITransactionQueryService
    {
        public Task<IEnumerable<TransactionEntity>> GetUserTransactionsAsync(string userId, DateRange dateRange);
    }
}
