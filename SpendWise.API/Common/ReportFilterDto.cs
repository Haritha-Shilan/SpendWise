namespace SpendWise.API.Common
{
    public class ReportFilterDto
    {
        public ReportPeriod Period { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
