namespace SpendWise.API.Features.Transaction
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize(Roles ="User")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IWebHostEnvironment _environment;
        public TransactionController(ITransactionService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment= environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                _=>BadRequest(result.Error)
            };
        }

       
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetAllByFilter([FromQuery] TransactionFilterDto filter)
        {
            var result = await _service.GetAllByFilterAsync(filter);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                _ => BadRequest(result.Error)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransactionResponseDto>> GetById(int id)
        {
            var result= await _service.GetByIdAsync(id);

            return result.Status switch
            {
                ServiceResultStatus.Success => Ok(result.Data),
                ServiceResultStatus.NotFound => NotFound(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TransactionCreateDto dto)
        {
            var result= await _service.CreateAsync(dto);

            return result.Status switch
            {
                ServiceResultStatus.Success =>
                CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data),

                ServiceResultStatus.Conflict =>
                    Conflict(result.Error),

                ServiceResultStatus.ValidationError =>
                    BadRequest(result.Error),

                _ => BadRequest(result.Error)
            };
        }


        [HttpPut]
        public async Task<IActionResult> Update(int id,[FromForm] TransactionUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id,dto);

            return result.Status switch
            {
                ServiceResultStatus.Success => NoContent(),
                ServiceResultStatus.NotFound => NotFound(result.Error),
                ServiceResultStatus.ValidationError => BadRequest(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result= await _service.DeleteAsync(id);

            return result.Status switch
            {
                ServiceResultStatus.Success => NoContent(),
                ServiceResultStatus.NotFound => NotFound(result.Error),
                _ => BadRequest(result.Error)
            };
        }

        [HttpGet("{id:int}/attachment")]
        public async Task<IActionResult> GetAttachment(int id)
        {
            var result = await _service.GetAttachmentAsync(id);

            return result.Status switch
            {
                ServiceResultStatus.Success =>
                    PhysicalFile(Path.Combine(
                    _environment.WebRootPath,
                    result.Data!.FilePath),
                    result.Data.ContentType,
                    result.Data.FileName),

                ServiceResultStatus.NotFound =>
                    NotFound(result.Error),

                _ => BadRequest(result.Error)
            };
        }
    
    }
}
