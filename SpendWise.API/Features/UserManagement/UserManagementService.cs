namespace SpendWise.API.Features.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResult<IEnumerable<UserManagementResponseDto>>> GetUsersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");

            var result = users
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new UserManagementResponseDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email ?? string.Empty,
                    CreatedAt = x.CreatedAt,
                    IsActive = x.IsActive
                });

            return new ServiceResult<IEnumerable<UserManagementResponseDto>>
            { 
                Status=ServiceResultStatus.Success,
                Data = result
            };              
        }

        public async Task<ServiceResult<bool>> SetUserActiveStatusAsync(string id,bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound
                };
            }

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }
    }
}