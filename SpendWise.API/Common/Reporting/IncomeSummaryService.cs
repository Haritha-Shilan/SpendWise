using TransactionEntity = SpendWise.Domain.Entities.Transaction;

namespace SpendWise.API.Common.Reporting
{
    public class IncomeSummaryService : IIncomeSummaryService
    {
        public IEnumerable<IncomeSummaryDto> GetIncomeSummary(IEnumerable<TransactionEntity> transactions)
        {
            return transactions
                .Where(x =>
                    x.UserCategory!.TypeId ==
                    (int)CategoryType.Income)
                .GroupBy(x => x.UserCategory!.Name)
                .Select(group => new IncomeSummaryDto
                {
                    CategoryName = group.Key,
                    TransactionCount = group.Count(),
                    Amount = group.Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}