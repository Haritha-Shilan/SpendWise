namespace SpendWise.API.Features.AdminDashboard
{
    public interface IAdminDashboardService
    {
        Task<ServiceResult<AdminDashboardResponseDto>> GetDashboard();
    }
}
