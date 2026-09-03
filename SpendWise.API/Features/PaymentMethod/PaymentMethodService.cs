using PaymentMethodEntity = SpendWise.Domain.Entities.PaymentMethod;
namespace SpendWise.API.Features.PaymentMethod
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethodEntity> _repository;
        private readonly IMapper _mapper;
        public PaymentMethodService(IRepository<PaymentMethodEntity> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult<bool>> ActivateAsync(int id) => await ToggleStatusAsync(id, true);

        public async Task<ServiceResult<PaymentMethodResponseDto>> CreateAsync(PaymentMethodCreateDto dto)
        {
            dto.Name = dto.Name.Trim();
            
            if(await IsPaymentMethodAlreadyExistsAsync(dto.Name))
            {
                return new ServiceResult<PaymentMethodResponseDto>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = "Payment method with the same name already exists."
                };
            }

            var paymentMethod = _mapper.Map<PaymentMethodEntity>(dto);
            paymentMethod.IsActive = true;

            await _repository.AddAsync(paymentMethod);

            return new ServiceResult<PaymentMethodResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<PaymentMethodResponseDto>(paymentMethod)
            };
        }

        public async Task<ServiceResult<bool>> DeactivateAsync(int id) => await ToggleStatusAsync(id, false);

        public async Task<ServiceResult<IEnumerable<PaymentMethodResponseDto>>> GetAllAsync()
        {
            var paymentMethods = await _repository.GetAllAsync();

            return new ServiceResult<IEnumerable<PaymentMethodResponseDto>>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<IEnumerable<PaymentMethodResponseDto>>(paymentMethods)
            };
        }

        public async Task<ServiceResult<PaymentMethodResponseDto>> GetByIdAsync(int id)
        {
            var paymentMethod = await _repository.GetByIdAsync(id);

            if(paymentMethod == null)
            {
                return new ServiceResult<PaymentMethodResponseDto>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Payment method not found."
                };
            }

            return new ServiceResult<PaymentMethodResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<PaymentMethodResponseDto>(paymentMethod)
            };
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, PaymentMethodUpdateDto dto)
        {
            dto.Name=dto.Name.Trim();
            var validationResult =await  ValidateUpdateAsync(id,dto);

            if(validationResult.Status!=ServiceResultStatus.Success)
            {
                return new ServiceResult<bool>
                {
                    Status = validationResult.Status,
                    Error = validationResult.Error
                };
            }

            var paymentMethod = validationResult.Data!;
            _mapper.Map(dto, paymentMethod);
            await _repository.UpdateAsync(paymentMethod);

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data=true
            };
        }

        private async Task<ServiceResult<bool>> ToggleStatusAsync(int id, bool isActive)
        {
            var paymentMethod = await _repository.GetByIdAsync(id);
            if (paymentMethod == null)
            {

                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Payment method not found."
                };
            }

            paymentMethod.IsActive = isActive;

            await _repository.UpdateAsync(paymentMethod);

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data = true
            };
        }

        #region ValidationMethods
        private async Task<bool> IsPaymentMethodAlreadyExistsAsync(string name,int? id=null)
        {
            var options = new QueryOptions<PaymentMethodEntity>();

            options.Filters.Add(x=>x.Name == name);

            if(id.HasValue)
            {
                options.Filters.Add(x => x.Id != id.Value);
            }

            var paymentMethods = await _repository.GetAllAsync(options);

            return paymentMethods.Any();
        }

        private async Task<ServiceResult<PaymentMethodEntity>> ValidateUpdateAsync(int id, PaymentMethodUpdateDto dto)
        {
            var paymentMethod = await _repository.GetByIdAsync(id);

            if (paymentMethod == null)
            {
                return new ServiceResult<PaymentMethodEntity>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Payment method not found."
                };
            }

            if(await IsPaymentMethodAlreadyExistsAsync(dto.Name,id))
            {
                return new ServiceResult<PaymentMethodEntity>
                {
                    Status = ServiceResultStatus.Conflict,
                    Error = "Payment method with the same name already exists."
                };
            }

            return new ServiceResult<PaymentMethodEntity>
            {
                Status = ServiceResultStatus.Success,
                Data= paymentMethod
            };
        }
        #endregion
    }
}
