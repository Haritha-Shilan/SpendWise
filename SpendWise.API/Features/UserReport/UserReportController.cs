namespace SpendWise.API.Features.UserReports
{
    [ApiController]
    [Route("api/user/reports")]
    [Authorize(Roles = "User")]
    public class UserReportController : ControllerBase
    {
        private readonly IUserReportService _reportService;

        public UserReportController(
            IUserReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(
            [FromQuery] ReportFilterDto filter)
        {
            var result = await _reportService.GetReport(filter);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                _ => BadRequest(result.Error)
            };
        }
    }
}