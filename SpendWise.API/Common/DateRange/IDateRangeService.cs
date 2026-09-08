namespace SpendWise.API.Common.DateRange
{
    public interface IDateRangeService
    {
        public ServiceResult<DateRange> GetDateRange(ReportPeriod period, DateTime? fromDate, DateTime? toDate);
    }
}
