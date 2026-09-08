namespace SpendWise.API.Features.UserReport
{
    public interface IUserReportService
    {
        public Task<ServiceResult<UserReportResponseDto>> GetReport(ReportFilterDto filter);
    }
}
