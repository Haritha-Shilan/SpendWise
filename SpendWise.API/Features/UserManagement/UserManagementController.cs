namespace SpendWise.API.Features.UserManagement
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _service;

        public UserManagementController(IUserManagementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _service.GetUsersAsync();

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                _ => BadRequest(result.Error)
            };
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateUser(string id)
        {
            var result = await _service.SetUserActiveStatusAsync(id, true);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                ServiceResultStatus.NotFound => NotFound(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(string id)
        {
            var result = await _service.SetUserActiveStatusAsync(id, false);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                ServiceResultStatus.NotFound => NotFound(result.Error),
                _ => BadRequest(result.Error)
            };
        }
    }
}