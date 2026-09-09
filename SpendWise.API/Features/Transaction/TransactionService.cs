namespace SpendWise.API.Features.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<TransactionEntity> _repository;
        private readonly IRepository<UserCategoryEntity> _userCategoryRepository;
        private readonly IRepository<PaymentMethodEntity> _paymentMethodRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRepository<TransactionAttachmentEntity> _attachmentRepository;

        private const string AttachmentNotFoundMsg = "Attachment not found.";
        private const string InvalidUserCategoryMsg = "Invalid User category.";
        private const string InvalidPaymentMethodMsg = "Invalid Payment method.";
        private const string TransactionNotFoundMsg = "Transaction not found.";
        private const string IncludeEntities = "UserCategory,UserCategory.CategoryTypeMaster,PaymentMethod,TransactionAttachment";
        private const string InvalidAttachmentOptionMsg = "Attachment cannot be uploaded and removed at the same time.";

        public TransactionService(IRepository<TransactionEntity> repository
                                , IRepository<UserCategoryEntity> userCategoryRepository
                                , IRepository<PaymentMethodEntity> paymentMethodRepository          
                                , IMapper mapper
                                , IUnitOfWork unitOfWork
                                , IHttpContextAccessor httpContextAccessor
                                , IFileStorageService fileStorageService
                                , IRepository<TransactionAttachmentEntity> attachmentRepository)
        {
            _repository = repository;
            _userCategoryRepository = userCategoryRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userId = GetCurrentUserId();
            _fileStorageService= fileStorageService;
            _attachmentRepository = attachmentRepository;
        }
        public async Task<ServiceResult<TransactionResponseDto>> CreateAsync(TransactionCreateDto dto)
        {
            var validationResult = await ValidateCreateAsync(dto);
            if(validationResult.Status!=ServiceResultStatus.Success)
            {
                return new ServiceResult<TransactionResponseDto>
                {
                    Status = validationResult.Status,
                    Error = validationResult.Error
                };
            }

            int transactionId = await CreateTransactionAsync(dto); ;
            var createdTransaction = await GetByIdAsync(transactionId);
            
            return new ServiceResult<TransactionResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = createdTransaction.Data
            };
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var entityResult = await GetTransactionEntityByIdAsync(id);
            if (entityResult.Status != ServiceResultStatus.Success)
            {
                return new ServiceResult<bool>
                {
                    Status = entityResult.Status,
                    Error = entityResult.Error
                };
            }
            await _unitOfWork.BeginTransactionAsync();
            string? attachmentFilePath = null;
            try
            {
                var entity = entityResult.Data!;
                if(entity.TransactionAttachment!=null)
                {
                    attachmentFilePath = entity.TransactionAttachment.FilePath;

                    await _attachmentRepository.DeleteAsync(entity.TransactionAttachment.Id);
                }

                await _repository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                if (!string.IsNullOrEmpty(attachmentFilePath))
                {
                    await _fileStorageService.DeleteAsync(attachmentFilePath);
                }

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

        public async Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllAsync() =>
            await GetAllTransactionAsync();

        public async Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllByFilterAsync(TransactionFilterDto dto)=>
            await GetAllTransactionAsync(filterDto:dto);

        public async Task<ServiceResult<TransactionResponseDto>> GetByIdAsync(int id)
        {
            var options = new QueryOptions<TransactionEntity>();
            options.Filters.Add(x=>x.UserId==_userId);
            options.Includes = IncludeEntities;

            var transaction = await _repository.GetByIdAsync(id,options);
            if(transaction == null)
            {
                return new ServiceResult<TransactionResponseDto>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = TransactionNotFoundMsg
                };
            }

            return new ServiceResult<TransactionResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<TransactionResponseDto>(transaction)
            };
        }

        public async Task<ServiceResult<TransactionEntity>> GetTransactionEntityByIdAsync(int id)
        {
            var options = new QueryOptions<TransactionEntity>();
            options.Filters.Add(x => x.UserId == _userId);
            options.Includes = IncludeEntities;

            var transaction = await _repository.GetByIdAsync(id, options);
            if (transaction == null)
            {
                return new ServiceResult<TransactionEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = TransactionNotFoundMsg
                };
            }

            return new ServiceResult<TransactionEntity>
            {
                Status = ServiceResultStatus.Success,
                Data = transaction
            };
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, TransactionUpdateDto dto)
        {
            var validationResult = await ValidateUpdateAsync(id, dto);

            if (validationResult.Status != ServiceResultStatus.Success)
            {
                return new ServiceResult<bool>
                {
                    Status = validationResult.Status,
                    Error = validationResult.Error
                };
            }

            var transaction = validationResult.Data!;
            await UpdateTransactionAsync(id,dto,transaction);

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }

        private async Task<bool> UpdateTransactionAsync(int id, TransactionUpdateDto dto,TransactionEntity entity)
        {
            await _unitOfWork.BeginTransactionAsync();

            string? oldFilePath = null;
            string? newFilePath = null;
            try
            {
                _mapper.Map(dto, entity);

                (oldFilePath, newFilePath) =
                    await UpdateAttachmentAsync(dto.Attachment, entity, dto.RemoveAttachment);

                await _repository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    await _fileStorageService.DeleteAsync(oldFilePath);
                }

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (!string.IsNullOrEmpty(newFilePath))
                {
                    await _fileStorageService.DeleteAsync(newFilePath);
                }

                throw;           
            }
        }

        private async Task<(string? OldFilePath, string? NewFilePath)> UpdateAttachmentAsync(IFormFile? file,TransactionEntity entity,bool removeAttachment)
        {
            string? oldFilePath=null;
            string? newFilePath = null;

            if (file != null)
            {
                oldFilePath= await DeleteAttachmentAsync(entity);
                newFilePath = await SaveAttachmentAsync(entity.Id, file);
               
            }
            else if (removeAttachment)
            {
                oldFilePath=await DeleteAttachmentAsync(entity);
            }

            return (oldFilePath,newFilePath);
        }

        private async Task<string?> DeleteAttachmentAsync(TransactionEntity entity)
        {
            if (entity.TransactionAttachment == null)
                return null;

            var oldFilePath = entity.TransactionAttachment.FilePath;

            await _attachmentRepository.DeleteAsync(
                entity.TransactionAttachment.Id);

            return oldFilePath;
        }

        private async Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllTransactionAsync(TransactionFilterDto? filterDto=null)
        {
            var options = new QueryOptions<TransactionEntity>();
            options.Filters.Add(x => x.UserId == _userId);
            options.Includes = IncludeEntities;

            options.OrderDescending = true;
            options.OrderBy = x => x.Date;

            if (filterDto != null)
                AddFilters(options, filterDto);

            var transactions = await _repository.GetAllAsync(options);

            return new ServiceResult<IEnumerable<TransactionResponseDto>>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions)
            };
        }

        private void AddFilters(QueryOptions<TransactionEntity> options, TransactionFilterDto filter)
        {
            if (filter == null)
                return;

            if(!string.IsNullOrEmpty(filter.FilterText))
            {
                var text = filter.FilterText.Trim()??string.Empty;
                options.Filters.Add(x=>
                x.Description.Contains(text) ||
                (x.UserCategory !=null && x.UserCategory!.Name.Contains(text))||
                (x.UserCategory != null && x.UserCategory.CategoryTypeMaster!=null && x.UserCategory.CategoryTypeMaster.Name.Contains(text))||
                (x.PaymentMethod!=null && x.PaymentMethod.Name.Contains(text)));
            }

            if(filter.FromDate.HasValue)
            {
                options.Filters.Add(x=>x.Date>=filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                options.Filters.Add(x => x.Date <= filter.ToDate.Value);
            }

            if(filter.UserCategoryId.HasValue)
            {
                options.Filters.Add(x => x.UserCategoryId == filter.UserCategoryId);
            }

            if (filter.CategoryTypeId.HasValue)
            {
                options.Filters.Add(x =>
                x.UserCategory != null && x.UserCategory.TypeId == filter.CategoryTypeId);
            }

            if (filter.PaymentMethodId.HasValue)
            {
                options.Filters.Add(x =>x.PaymentMethodId == filter.PaymentMethodId);
            }

            if (filter.MinAmount.HasValue)
            {
                options.Filters.Add(x => x.Amount >= filter.MinAmount);
            }


            if (filter.MaxAmount.HasValue)
            {
                options.Filters.Add(x => x.Amount <= filter.MaxAmount);
            }
        }
        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException();
        }

        private async Task<int> CreateTransactionAsync(TransactionCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            string? savedFilePath = null;

            try
            {
                var transaction = _mapper.Map<TransactionEntity>(dto);
                transaction.UserId = _userId;

                await _repository.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                if (dto.Attachment != null)
                {
                    savedFilePath = await SaveAttachmentAsync(
                        transaction.Id,
                        dto.Attachment);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return transaction.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (!string.IsNullOrEmpty(savedFilePath))
                {
                    await _fileStorageService.DeleteAsync(savedFilePath);
                }

                throw;
            }
        }

        private async Task<string?> SaveAttachmentAsync(int transactionId,IFormFile file)
        {
            string savedFilePath = await _fileStorageService.SaveAsync(file);

            var attachment = new TransactionAttachmentEntity
            {
                TransactionId = transactionId,
                FileName = file.FileName,
                FilePath = savedFilePath,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            await _attachmentRepository.AddAsync(attachment);

            return savedFilePath;
        }

        public async Task<ServiceResult<TransactionAttachmentEntity>> GetAttachmentAsync(int id)
        {
            var entityResult = await GetTransactionEntityByIdAsync(id);

            if (entityResult.Status != ServiceResultStatus.Success)
            {
                return new ServiceResult<TransactionAttachmentEntity>
                {
                    Status = entityResult.Status,
                    Error = entityResult.Error
                };
            }

            var transaction = entityResult.Data!;

            if (transaction.TransactionAttachment == null)
            {
                return new ServiceResult<TransactionAttachmentEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error =AttachmentNotFoundMsg
                };
            }

            return new ServiceResult<TransactionAttachmentEntity>
            {
                Status = ServiceResultStatus.Success,
                Data = transaction.TransactionAttachment
            };
        }

        #region ValidationMethods
        private async Task<ServiceResult<bool>> ValidateCreateAsync(TransactionCreateDto dto)
        {
            if(!await IsUserCategoryValidAsync(dto.UserCategoryId))
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = InvalidUserCategoryMsg
                };
            }

            if (!await IsPaymentMethodValidAsync(dto.PaymentMethodId))
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = InvalidPaymentMethodMsg
                };
            }

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };

        }

        private async Task<ServiceResult<TransactionEntity>> ValidateUpdateAsync(int id,TransactionUpdateDto dto)
        {
            var options = new QueryOptions<TransactionEntity>();
            options.Filters.Add(x=>x.UserId==_userId);
            options.Includes = IncludeEntities;

            var transaction = await _repository.GetByIdAsync(id,options);
            if(transaction==null)
            {
                return new ServiceResult<TransactionEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = TransactionNotFoundMsg
                };
            }

            if (dto.UserCategoryId != transaction.UserCategoryId &&
                !await IsUserCategoryValidAsync(dto.UserCategoryId))
            {
                return new ServiceResult<TransactionEntity>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidUserCategoryMsg
                };
            }

            if (dto.PaymentMethodId != transaction.PaymentMethodId&&
                !await IsPaymentMethodValidAsync(dto.PaymentMethodId))
            {
                return new ServiceResult<TransactionEntity>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidPaymentMethodMsg
                };
            }

            if(dto.Attachment!=null && dto.RemoveAttachment)
            {
                return new ServiceResult<TransactionEntity>
                {
                    Status = ServiceResultStatus.ValidationError,
                    Error = InvalidAttachmentOptionMsg
                };
            }

            return new ServiceResult<TransactionEntity>
            {
                Status = ServiceResultStatus.Success,
                Data = transaction
            };

        }

        private async Task<bool> IsUserCategoryValidAsync(int id)
        {
            var options = new QueryOptions<UserCategoryEntity>();
            options.Filters.Add(x=>
                x.IsActive &&
                x.UserId==_userId );

            var userCategory = await _userCategoryRepository.GetByIdAsync(id,options);
            if (userCategory == null) return false;
            return true;
        }

        private async Task<bool> IsPaymentMethodValidAsync(int id)
        {
            var options = new QueryOptions<PaymentMethodEntity>();
            options.Filters.Add(x =>x.IsActive );

            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, options);
            if (paymentMethod == null) return false;
            return true;
        }
        #endregion
    }
}
