using TransactionEntity = SpendWise.Domain.Entities.Transaction;
namespace SpendWise.API.Common.Reporting
{
    public interface IExpenseByCategoryService
    {
        public IEnumerable<ExpenseByCategoryDto> GetExpenseByCategory(IEnumerable<TransactionEntity> transactions);
    }
}
