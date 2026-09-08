namespace SpendWise.API.Features.UserDashboard.DTOs
{
    public class DashboardResponseDto
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<MonthlySummaryDto> MonthlySummary { get; set; }=
            new List<MonthlySummaryDto>();

        public IEnumerable<ExpenseByCategoryDto> ExpenseByCategory { get; set; }
            = new List<ExpenseByCategoryDto>();

        public IEnumerable<RecentTransactionDto> RecentTransactions { get; set; }
            = new List<RecentTransactionDto>();
    }

    public class RecentTransactionDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Description {  get; set; } = string.Empty;
    }
}
