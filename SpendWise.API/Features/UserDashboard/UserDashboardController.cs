namespace SpendWise.API.Features.UserDashboard
{
    [ApiController]
    [Route("api/user/dashboard")]
    [Authorize(Roles = "User")]
    public class UserDashboardController : ControllerBase
    {
        private readonly IUserDashboardService _dashboardService;

        public UserDashboardController(IUserDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] ReportFilterDto filter)
        {
            var result = await _dashboardService.GetDashboard(filter);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                _ => BadRequest(result.Error)
            };
        }
    }
}
