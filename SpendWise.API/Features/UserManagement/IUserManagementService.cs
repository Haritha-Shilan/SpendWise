namespace SpendWise.API.Features.UserManagement
{
    public interface IUserManagementService
    {
        Task<ServiceResult<IEnumerable<UserManagementResponseDto>>> GetUsersAsync();

        Task<ServiceResult<bool>> SetUserActiveStatusAsync(string id, bool isActive);
    }
}