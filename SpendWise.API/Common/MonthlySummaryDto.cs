namespace SpendWise.API.Common
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string MonthName { get; set; } = string.Empty;

        public decimal Income { get; set; }

        public decimal Expense { get; set; }
    }
}
