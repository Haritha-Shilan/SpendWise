namespace SpendWise.API.Features.UserCategory
{
    public class UserCategoryService : IUserCategoryService
    {
        private readonly IRepository<UserCategoryEntity> _repository;
        private readonly IRepository<CategoryTypeMaster> _categoryTypeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        private const string UserCategoryNotFoundMsg = "User category not found.";
        private const string InvalidCategoryTypeMsg = "Invalid category type.";
        private const string ConflictMsg = "A category with the same name already exists for this category type.";
        public UserCategoryService(IRepository<UserCategoryEntity> repository,IRepository<CategoryTypeMaster> categoryTypeRepository,IMapper mapper,
            IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _categoryTypeRepository = categoryTypeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userId = GetCurrentUserId();
        }
        public async Task<ServiceResult<bool>> ActivateAsync(int id) =>
            await ToggleStatusAsync(id, true);

        public async Task<ServiceResult<UserCategoryResponseDto>> CreateAsync(UserCategoryCreateDto dto)
        {
            dto.Name = dto.Name.Trim();
            var validationResult = await ValidateCreateAsync(dto);
            if (validationResult.Status != ServiceResultStatus.Success)
            {
                return validationResult;
            }

            //Map Dto to entity
            var userCategory = _mapper.Map<UserCategoryEntity>(dto);
            userCategory.IsActive = true;
            userCategory.UserId = _userId;

            await _repository.AddAsync(userCategory);
            await _unitOfWork.SaveChangesAsync();

            var createdCategory = await GetUserCategoryByIdAsync(userCategory.Id);

            return new ServiceResult<UserCategoryResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<UserCategoryResponseDto>(createdCategory)
            };
        }

        public async Task<ServiceResult<bool>> DeactivateAsync(int id)=>
            await ToggleStatusAsync(id, false);

        public async Task<ServiceResult<IEnumerable<UserCategoryResponseDto>>> GetAllActiveAsync()=>
            await GetAllCategoryAsync(isActive:true);


        public async Task<ServiceResult<IEnumerable<UserCategoryResponseDto>>> GetAllAsync()=>
            await GetAllCategoryAsync();


        public async Task<ServiceResult<UserCategoryResponseDto>> GetByIdAsync(int id)
        {
            var userCategory = await GetUserCategoryByIdAsync(id);
            if(userCategory==null)
            {
                return new ServiceResult<UserCategoryResponseDto>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = UserCategoryNotFoundMsg
                };
            }

            return new ServiceResult<UserCategoryResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<UserCategoryResponseDto>(userCategory)
            };
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, UserCategoryUpdateDto dto)
        {
            dto.Name = dto.Name.Trim();

            var validationResult = await ValidateUpdateAsync(id, dto);

            if (validationResult.Status != ServiceResultStatus.Success)
            {
                return new ServiceResult<bool>
                {
                    Status = validationResult.Status,
                    Error = validationResult.Error
                };
            }

            var userCategory = validationResult.Data!;

            _mapper.Map(dto, userCategory);

            await _repository.UpdateAsync(userCategory);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }
        private async Task<ServiceResult<IEnumerable<UserCategoryResponseDto>>> GetAllCategoryAsync(bool? isActive=null)
        {
            var options = new QueryOptions<UserCategoryEntity>();

            if(isActive.HasValue)
            {
                options.Filters.Add(x=>x.IsActive==isActive.Value);
            }

            options.Filters.Add(x => x.UserId == _userId);
            options.Includes = "CategoryTypeMaster";

            var userCategories = await _repository.GetAllAsync(options);

            return new ServiceResult<IEnumerable<UserCategoryResponseDto>>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<IEnumerable<UserCategoryResponseDto>>(userCategories)
            };
        }
        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException();
        }

        private async Task<ServiceResult<bool>> ToggleStatusAsync(int id, bool isActive)
        {
            var userCategory = await GetUserCategoryByIdAsync(id);
            if (userCategory == null)
            {

                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = UserCategoryNotFoundMsg
                };
            }

            userCategory.IsActive = isActive;

            await _repository.UpdateAsync(userCategory);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }

        private async Task<UserCategoryEntity?> GetUserCategoryByIdAsync(int id)
        {
            var options = new QueryOptions<UserCategoryEntity>();
            options.Filters.Add(x => x.UserId == _userId);
            options.Includes = "CategoryTypeMaster";

            return await _repository.GetByIdAsync(id, options);
        }

        #region ValidationMethods
        private async Task<bool> IsCategoryTypeExistsAsync(int id)
        {
            var categoryType = await _categoryTypeRepository.GetByIdAsync(id);
            return categoryType != null;
        }

        private async Task<bool> IsCategoryAlreadyExistsAsync(string name, int categoryTypeId, int? categoryId = null)
        {
            var options = new QueryOptions<UserCategoryEntity>();

            options.Filters.Add(x => x.Name == name);
            options.Filters.Add(x => x.TypeId == categoryTypeId);
            options.Filters.Add(x => x.UserId == _userId);

            if (categoryId.HasValue)
                options.Filters.Add(x => x.Id != categoryId.Value);

            var categories = await _repository.GetAllAsync(options);

            return categories.Any();
        }

        private async Task<ServiceResult<UserCategoryResponseDto>> ValidateCreateAsync(UserCategoryCreateDto dto)
        {
            //Validate CategoryType

            if (!await IsCategoryTypeExistsAsync(dto.TypeId))
            {
                return new ServiceResult<UserCategoryResponseDto>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidCategoryTypeMsg
                };
            }

            //Check duplicate

            if (await IsCategoryAlreadyExistsAsync(dto.Name, dto.TypeId))
            {
                return new ServiceResult<UserCategoryResponseDto>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = ConflictMsg
                };
            }

            return new ServiceResult<UserCategoryResponseDto>
            {
                Status = ServiceResultStatus.Success
            };
        }

        private async Task<ServiceResult<UserCategoryEntity>> ValidateUpdateAsync(int id, UserCategoryUpdateDto dto)
        {
            //Check Category
            var userCategory = await GetUserCategoryByIdAsync(id);

            if (userCategory == null)
            {
                return new ServiceResult<UserCategoryEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = UserCategoryNotFoundMsg
                };
            }

            //Validate CategoryType
            if (!await IsCategoryTypeExistsAsync(dto.TypeId))
            {
                return new ServiceResult<UserCategoryEntity>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidCategoryTypeMsg
                };
            }

            //Check duplicate
            if (await IsCategoryAlreadyExistsAsync(dto.Name, dto.TypeId, id))
            {
                return new ServiceResult<UserCategoryEntity>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = ConflictMsg
                };
            }

            return new ServiceResult<UserCategoryEntity>
            {
                Status = ServiceResultStatus.Success,
                Data = userCategory
            };
        }
        #endregion
    }
}
