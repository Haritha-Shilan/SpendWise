namespace SpendWise.API.Features.UserCategory
{
    public interface IUserCategoryService
    {
        public Task<ServiceResult<IEnumerable<UserCategoryResponseDto>>> GetAllAsync();

        public Task<ServiceResult<IEnumerable<UserCategoryResponseDto>>> GetAllActiveAsync();

        public Task<ServiceResult<UserCategoryResponseDto>> GetByIdAsync(int id);

        public Task<ServiceResult<UserCategoryResponseDto>> CreateAsync(UserCategoryCreateDto dto);

        public Task<ServiceResult<bool>> UpdateAsync(int id,UserCategoryUpdateDto dto);

        public Task<ServiceResult<bool>> ActivateAsync(int id);

        public Task<ServiceResult<bool>> DeactivateAsync(int id);
    }
}
