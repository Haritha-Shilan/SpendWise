namespace SpendWise.API.Features.Transaction
{
    public class TransactionQueryService : ITransactionQueryService
    {
        private readonly IRepository<TransactionEntity> _transactionRepository;

        private const string IncludeEntities =
            "UserCategory,UserCategory.CategoryTypeMaster,PaymentMethod,TransactionAttachment";

        public TransactionQueryService(
            IRepository<TransactionEntity> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionEntity>> GetUserTransactionsAsync(
            string userId,
            DateRange dateRange)
        {
            var options = new QueryOptions<TransactionEntity>();

            options.Filters.Add(x => x.UserId == userId);

            options.Includes = IncludeEntities;

            if (dateRange.FromDate.HasValue)
            {
                options.Filters.Add(x =>
                    x.Date >= dateRange.FromDate.Value);
            }

            if (dateRange.ToDate.HasValue)
            {
                options.Filters.Add(x =>
                    x.Date < dateRange.ToDate.Value);
            }

            options.OrderBy = x => x.Date;
            options.OrderDescending = true;

            return await _transactionRepository.GetAllAsync(options);
        }
    }
}