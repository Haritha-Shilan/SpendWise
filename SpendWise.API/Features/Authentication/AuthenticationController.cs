namespace SpendWise.API.Features.Authentication
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);

            if(result.Status== ServiceResultStatus.Conflict)
                return Conflict(result.Error);

            if (result.Status == ServiceResultStatus.Success)
                return StatusCode(StatusCodes.Status201Created,result.Data);

           return BadRequest(result.Error);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);

            if(result.Status!=ServiceResultStatus.Success)
                return Unauthorized(result.Error);

            return Ok(result.Data);
        }
    }
}
