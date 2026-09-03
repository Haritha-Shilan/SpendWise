using SpendWise.API.Features.Authentication.DTOs;

namespace SpendWise.API.Features.Authentication
{
    public interface IAuthenticationService
    {
        public Task<ServiceResult<bool>> RegisterAsync(RegisterDto dto);

        public Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
}
