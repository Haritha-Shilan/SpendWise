namespace SpendWise.API.Common.Reporting
{
    public interface IMonthlySummaryService
    {
        public IEnumerable<MonthlySummaryDto> GetMonthlySummary(IEnumerable<TransactionEntity> transactions, ReportPeriod period, DateRangeModal dateRange);
    }
}
