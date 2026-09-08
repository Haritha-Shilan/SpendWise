namespace SpendWise.API.Features.AdminDashboard;

[ApiController]
[Route("api/admin/dashboard")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : ControllerBase
{
    private readonly IAdminDashboardService _dashboardService;

    public AdminDashboardController(
        IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _dashboardService.GetDashboard();

        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Data),
            _ => BadRequest(result.Error)
        };
    }
}