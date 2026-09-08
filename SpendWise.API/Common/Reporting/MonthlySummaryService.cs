using TransactionEntity = SpendWise.Domain.Entities.Transaction;
using DateRangeModal = SpendWise.API.Common.DateRange.DateRange;

namespace SpendWise.API.Common.Reporting
{
    public class MonthlySummaryService : IMonthlySummaryService
    {
        public IEnumerable<MonthlySummaryDto> GetMonthlySummary(IEnumerable<TransactionEntity> transactions, ReportPeriod period, DateRangeModal dateRange)
        {
            if (!transactions.Any() && !dateRange.FromDate.HasValue)
            {
                return new List<MonthlySummaryDto>();
            }

            var fullDateRange = GetDateRange(transactions, dateRange);

            var firstMonth = fullDateRange.FromDate!.Value;
            var lastMonth = fullDateRange.ToDate!.Value;

            if (period == ReportPeriod.ThisYear)
            {
                var currentMonth = new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);

                if (lastMonth > currentMonth)
                {
                    lastMonth = currentMonth;
                }
            }

            var monthlySummary = new List<MonthlySummaryDto>();

            for (var month = firstMonth;
                 month <= lastMonth;
                 month = month.AddMonths(1))
            {
                var monthTransactions = transactions
                    .Where(x =>
                        x.Date.Year == month.Year &&
                        x.Date.Month == month.Month);

                var income = monthTransactions
                    .Where(x =>
                        x.UserCategory!.TypeId ==
                        (int)CategoryType.Income)
                    .Sum(x => x.Amount);

                var expense = monthTransactions
                    .Where(x =>
                        x.UserCategory!.TypeId ==
                        (int)CategoryType.Expense)
                    .Sum(x => x.Amount);

                monthlySummary.Add(new MonthlySummaryDto
                {
                    Year = month.Year,
                    Month = month.Month,
                    MonthName = month.ToString("MMM"),
                    Income = income,
                    Expense = expense
                });
            }

            return monthlySummary;
        }

        private DateRangeModal GetDateRange(IEnumerable<TransactionEntity> transactions, DateRangeModal dateRange)
        {
            if (dateRange.FromDate.HasValue)
            {
                return new DateRangeModal
                {
                    FromDate = new DateTime(
                        dateRange.FromDate.Value.Year,
                        dateRange.FromDate.Value.Month,
                        1),

                    ToDate = new DateTime(
                        dateRange.ToDate!.Value.Year,
                        dateRange.ToDate.Value.Month,
                        1)
                        .AddMonths(-1)
                };
            }

            return new DateRangeModal
            {
                FromDate = new DateTime(
                    transactions.Min(x => x.Date).Year,
                    transactions.Min(x => x.Date).Month,
                    1),

                ToDate = new DateTime(
                    transactions.Max(x => x.Date).Year,
                    transactions.Max(x => x.Date).Month,
                    1)
            };
        }
    }
}