namespace SpendWise.API.Features.Notification
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(Roles ="Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (result.Status == ServiceResultStatus.Success)
                return Ok(result.Data);

            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result.Status == ServiceResultStatus.NotFound)
                return NotFound(result.Error);

            if (result.Status == ServiceResultStatus.Success)
                return Ok(result.Data);

            return BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponseDto>> Create(NotificationCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if(result.Status==ServiceResultStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
            }

            return BadRequest(result.Error);
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _service.MarkAsReadAsync(id);

            if (result.Status == ServiceResultStatus.NotFound)
                return NotFound(result.Error);

            if (result.Status == ServiceResultStatus.Success)
                return NoContent();

            return BadRequest(result.Error);
        }
    }
}
