using CategoryMasterEntity = SpendWise.Domain.Entities.CategoryMaster;

namespace SpendWise.API.Features.CategoryMaster
{
    public class CategoryMasterService : ICategoryMasterService
    {
        private readonly IRepository<CategoryMasterEntity> _repository;
        private readonly IRepository<CategoryTypeMaster> _categoryTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryMasterService(IRepository<CategoryMasterEntity> repository, IRepository<CategoryTypeMaster> categoryTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository=repository;
            _categoryTypeRepository = categoryTypeRepository;
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }
        public async Task<ServiceResult<CategoryMasterResponseDto>> CreateAsync(CategoryMasterCreateDto dto)
        {
            dto.Name = dto.Name.Trim();
            var validationResult = await ValidateCreateAsync(dto);
            if(validationResult.Status!= ServiceResultStatus.Success)
            {
                return validationResult;
            }

            //Map Dto to entity
            var categoryMaster = _mapper.Map<CategoryMasterEntity>(dto);

            categoryMaster.IsActive = true;

            await _repository.AddAsync(categoryMaster);
            await _unitOfWork.SaveChangesAsync();

            var options = new QueryOptions<CategoryMasterEntity>
            {
                Includes = "CategoryTypeMaster"
            };

            var createdCategory = await _repository.GetByIdAsync(categoryMaster.Id,options);

            return new ServiceResult<CategoryMasterResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<CategoryMasterResponseDto>(createdCategory)
            };
        }

        public async Task<ServiceResult<bool>> ActivateAsync(int id)=> await ToggleStatusAsync(id,true);

        public async Task<ServiceResult<bool>> DeactivateAsync(int id) => await ToggleStatusAsync(id, false);

        private async Task<ServiceResult<bool>> ToggleStatusAsync(int id,bool isActive)
        {
            var categoryMaster = await _repository.GetByIdAsync(id);
            if (categoryMaster == null)
            {

                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error="Category not found."
                };
            }

            categoryMaster.IsActive = isActive;

            await _repository.UpdateAsync(categoryMaster);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }
        public async Task<ServiceResult<IEnumerable<CategoryMasterResponseDto>>> GetAllAsync()
        {
            var options = new QueryOptions<CategoryMasterEntity>
            {
                Includes = "CategoryTypeMaster"
            };

            var categoryMasters =await _repository.GetAllAsync(options);

            return new ServiceResult<IEnumerable<CategoryMasterResponseDto>>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<IEnumerable<CategoryMasterResponseDto>>(categoryMasters)
            };
        }

        public async Task<ServiceResult<CategoryMasterResponseDto>> GetByIdAsync(int id)
        {
            var options = new QueryOptions<CategoryMasterEntity>
            {
                Includes = "CategoryTypeMaster"
            };

            var categoryMaster = await _repository.GetByIdAsync(id,options);

            if(categoryMaster==null)
            {
                return new ServiceResult<CategoryMasterResponseDto>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Category not found."
                };
            }

            return new ServiceResult<CategoryMasterResponseDto>
            {
                Status= ServiceResultStatus.Success,
                Data = _mapper.Map<CategoryMasterResponseDto>(categoryMaster)
            };
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id,CategoryMasterUpdateDto dto)
        {
            dto.Name=dto.Name.Trim();

            var validationResult = await ValidateUpdateAsync(id, dto);

            if(validationResult.Status!=ServiceResultStatus.Success)
            {
                return new ServiceResult<bool>
                {
                    Status= validationResult.Status,
                    Error= validationResult.Error
                };
            }

            var categoryMaster = validationResult.Data!;

            _mapper.Map(dto, categoryMaster);

            await _repository.UpdateAsync(categoryMaster);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }

        #region ValidationMethods
        private async Task<bool> IsCategoryTypeExistsAsync(int id)
        {
            var categoryType =await _categoryTypeRepository.GetByIdAsync(id);
            return categoryType != null;
        }

        private async Task<bool> IsCategoryAlreadyExistsAsync(string name,int  categoryTypeId, int? categoryId = null)
        {
            var options = new QueryOptions<CategoryMasterEntity>();

            options.Filters.Add(x=>x.Name == name);
            options.Filters.Add(x => x.TypeId == categoryTypeId);
            
            if(categoryId.HasValue)
                options.Filters.Add(x=>x.Id!=categoryId.Value);

            var categories = await _repository.GetAllAsync(options);

            return categories.Any();
        }

        private async Task<ServiceResult<CategoryMasterResponseDto>> ValidateCreateAsync(CategoryMasterCreateDto dto)
        {
            //Validate CategoryType

            if (!await IsCategoryTypeExistsAsync(dto.TypeId))
            {
                return new ServiceResult<CategoryMasterResponseDto>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = "Invalid category type."
                };
            }

            //Check duplicate

            if (await IsCategoryAlreadyExistsAsync(dto.Name, dto.TypeId))
            {
                return new ServiceResult<CategoryMasterResponseDto>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = "A category with the same name already exists for this category type."
                };
            }

            return new ServiceResult<CategoryMasterResponseDto>
            {
                Status = ServiceResultStatus.Success
            };
        }

        private async Task<ServiceResult<CategoryMasterEntity>> ValidateUpdateAsync(int id,CategoryMasterUpdateDto dto)
        {

            //Check Category
            var categoryMaster = await _repository.GetByIdAsync(id);

            if (categoryMaster == null)
            {
                return new ServiceResult<CategoryMasterEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Category not found."
                };
            }

            //Validate CategoryType
            if (!await IsCategoryTypeExistsAsync(dto.TypeId))
            {
                return new ServiceResult<CategoryMasterEntity>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = "Invalid category type."
                };
            }

            //Check duplicate
            if (await IsCategoryAlreadyExistsAsync(dto.Name, dto.TypeId,id))
            {
                return new ServiceResult<CategoryMasterEntity>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = "A category with the same name already exists for this category type."
                };
            }

            return new ServiceResult<CategoryMasterEntity>
            {
                Status = ServiceResultStatus.Success,
                Data=categoryMaster
            };
        }
        #endregion
    }
}
