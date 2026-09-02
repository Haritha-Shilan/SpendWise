namespace SpendWise.API.Features.CategoryMaster
{
    public interface ICategoryMasterService
    {
        Task<ServiceResult<IEnumerable<CategoryMasterResponseDto>>> GetAllAsync();

        Task<ServiceResult<CategoryMasterResponseDto>> GetByIdAsync(int id);

        Task<ServiceResult<CategoryMasterResponseDto>> CreateAsync(CategoryMasterCreateDto dto);

        Task<ServiceResult<bool>> UpdateAsync(int id,CategoryMasterUpdateDto dto);

        Task<ServiceResult<bool>> ActivateAsync(int id);

        Task<ServiceResult<bool>> DeactivateAsync(int id);
    }
}
