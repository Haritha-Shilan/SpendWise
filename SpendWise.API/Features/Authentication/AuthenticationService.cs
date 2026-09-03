namespace SpendWise.API.Features.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private const string InvalidCredentialMsg = "Invalid email or password";
        private const string DuplicateEmailMsg = "A user with this email already exists.";
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var validationResult = await ValidateUserAsync(dto);

            if(validationResult.Status!=ServiceResultStatus.Success)
            {
                return new ServiceResult<LoginResponseDto>
                {
                    Status = validationResult.Status,
                    Error = validationResult.Error
                };
            }

            var user = validationResult.Data!;
            var tokenString = await CreateJWTTokenAsync(user);

            return new ServiceResult<LoginResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data= new LoginResponseDto { Token=tokenString}
            };
        }

        public async Task<ServiceResult<bool>> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = DuplicateEmailMsg
                };
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var createUserResult = await CreateUserAsync(dto);
                if (createUserResult.Status != ServiceResultStatus.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ServiceResult<bool>
                    {
                        Status = createUserResult.Status,
                        Error = createUserResult.Error
                    };
                }

                var user = createUserResult.Data!;

                var addToRoleResult= await AddToRoleAsync(user);

                if (addToRoleResult.Status != ServiceResultStatus.Success)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ServiceResult<bool>
                    {
                        Status = addToRoleResult.Status,
                        Error = addToRoleResult.Error
                    };
                }

                await _unitOfWork.CommitTransactionAsync();

                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.Success,
                    Data = true
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
           
        }

        private async Task<ServiceResult<ApplicationUser>> CreateUserAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new ServiceResult<ApplicationUser>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }

            return new ServiceResult<ApplicationUser>
            {
                Status = ServiceResultStatus.Success,
                Data = user
            };
        }

        private async Task<ServiceResult<bool>> AddToRoleAsync(ApplicationUser user)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = string.Join(", ", roleResult.Errors.Select(x => x.Description))
                };
            }

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }
        
        private async Task<ServiceResult<ApplicationUser>> ValidateUserAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new ServiceResult<ApplicationUser>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidCredentialMsg
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
            {
                return new ServiceResult<ApplicationUser>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidCredentialMsg
                };
            }

            return new ServiceResult<ApplicationUser>
            {
                Status = ServiceResultStatus.Success,
                Data = user
            };
        }

        private async Task<string> CreateJWTTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Name,user.FullName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
