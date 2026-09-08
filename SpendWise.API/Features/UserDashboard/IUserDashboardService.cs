namespace SpendWise.API.Features.UserDashboard
{
    public interface IUserDashboardService
    {
        public Task<ServiceResult<DashboardResponseDto>> GetDashboard(ReportFilterDto filter);
    }
}
