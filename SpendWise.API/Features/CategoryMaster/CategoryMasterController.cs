namespace SpendWise.API.Features.CategoryMaster
{
    [ApiController]
    [Route("api/categoryMaster")]
    public class CategoryMasterController : ControllerBase
    {
        private readonly ICategoryMasterService _service;
        public CategoryMasterController(ICategoryMasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryMasterResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (result.Status == ServiceResultStatus.Success)
                return Ok(result.Data);

            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryMasterResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if(result.Status == ServiceResultStatus.NotFound)
                return NotFound(result.Error);

            if (result.Status == ServiceResultStatus.Success)
                return Ok(result.Data);

            return BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryMasterResponseDto>> Create(CategoryMasterCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if(result.Status== ServiceResultStatus.Conflict)
                return Conflict(result.Error);

            if(result.Status== ServiceResultStatus.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
            }

            return BadRequest(result.Error);             
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryMasterUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result.Status == ServiceResultStatus.NotFound)
                return NotFound(result.Error);

            if (result.Status == ServiceResultStatus.Conflict)
                return Conflict(result.Error);

            if (result.Status == ServiceResultStatus.Success)
                return NoContent();
            
            return BadRequest(result.Error);
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id) => await ToggleState(id, true);

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id) => await ToggleState(id, false);

        private async Task<IActionResult> ToggleState(int id, bool activate)
        {
            var result = activate ?
                await _service.ActivateAsync(id) : await _service.DeactivateAsync(id);

            if (result.Status == ServiceResultStatus.NotFound)
                return NotFound(result.Error);

            if(result.Status==ServiceResultStatus.Success)
                return NoContent();

            return BadRequest(result.Error);
        }
    }
}
