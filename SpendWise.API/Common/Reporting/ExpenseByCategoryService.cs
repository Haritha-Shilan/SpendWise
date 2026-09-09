namespace SpendWise.API.Common.Reporting
{
    public class ExpenseByCategoryService : IExpenseByCategoryService
    {
        public IEnumerable<ExpenseByCategoryDto> GetExpenseByCategory(
            IEnumerable<TransactionEntity> transactions)
        {
            return transactions
                .Where(x =>
                    x.UserCategory!.TypeId ==
                    (int)CategoryType.Expense)
                .GroupBy(x => x.UserCategory!.Name)
                .Select(group => new ExpenseByCategoryDto
                {
                    CategoryName = group.Key,
                    Amount = group.Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}