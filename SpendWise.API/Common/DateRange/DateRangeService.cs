namespace SpendWise.API.Common.DateRange
{
    public class DateRangeService : IDateRangeService
    {
        public const string FromToDateRequiredMsg = "FromDate and ToDate are required for CustomRange.";
        public const string FromDateGreaterMsg = "FromDate cannot be greater than ToDate.";
        public const string InvalidRptPeriodMsg = "Invalid report period.";
        public ServiceResult<DateRange> GetDateRange(ReportPeriod period, DateTime? fromDate, DateTime? toDate)
        {
            var today = DateTime.Today;

            return period switch
            {
                ReportPeriod.ThisMonth =>
                  Success(new DateTime(today.Year, today.Month, 1),
                          new DateTime(today.Year, today.Month, 1).AddMonths(1)),

                ReportPeriod.LastMonth =>
                   Success(new DateTime(today.Year, today.Month, 1).AddMonths(-1),
                           new DateTime(today.Year, today.Month, 1)),

                ReportPeriod.ThisYear =>
                   Success(new DateTime(today.Year, today.Month, 1),
                           new DateTime(today.Year, today.Month, 1).AddYears(1)),

                ReportPeriod.AllTime =>
                    Success(null, null),


                ReportPeriod.CustomRange =>
                    ResolveCustomRange(fromDate, toDate),

                _ =>
                    ValidationError(InvalidRptPeriodMsg)
            };
        }

        private ServiceResult<DateRange> ResolveCustomRange(DateTime? fromDate,DateTime? toDate)
        {
            if (!fromDate.HasValue || !toDate.HasValue)
            {
                return ValidationError(FromToDateRequiredMsg);
            }

            if (fromDate.Value.Date > toDate.Value.Date)
            {
                return ValidationError(FromToDateRequiredMsg);
            }

            return Success(
                fromDate.Value.Date,
                toDate.Value.Date.AddDays(1));
        }

        private ServiceResult<DateRange> ValidationError(string message)
        {
            return new ServiceResult<DateRange>
            {
                Status = ServiceResultStatus.ValidationError,
                Error = message
            };
        }

        private ServiceResult<DateRange> Success(DateTime? from,DateTime? to)
        {
            return new ServiceResult<DateRange>
            {
                Status = ServiceResultStatus.Success,
                Data = new DateRange
                {
                    FromDate = from,
                    ToDate = to
                }
            };
        }
    }
}
