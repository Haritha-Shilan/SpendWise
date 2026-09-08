namespace SpendWise.API.Features.UserReport.DTOs
{
    public class UserReportResponseDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public decimal ExpenseRatio { get; set; }

        public IEnumerable<MonthlySummaryDto> MonthlySummary { get; set; }
            = new List<MonthlySummaryDto>();

        public IEnumerable<ExpenseByCategoryDto> ExpenseByCategory { get; set; }
            = new List<ExpenseByCategoryDto>();

        public IEnumerable<IncomeSummaryDto> IncomeSummary { get; set; }
            = new List<IncomeSummaryDto>();
    }

    public class IncomeSummaryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal Amount { get; set; }
    }
}
