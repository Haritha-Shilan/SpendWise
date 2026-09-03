namespace SpendWise.API.Features.PaymentMethod
{
    public interface IPaymentMethodService
    {
        public Task<ServiceResult<IEnumerable<PaymentMethodResponseDto>>> GetAllAsync();

        public Task<ServiceResult<PaymentMethodResponseDto>> GetByIdAsync(int id);

        public Task<ServiceResult<PaymentMethodResponseDto>> CreateAsync(PaymentMethodCreateDto dto);

        public Task<ServiceResult<bool>> UpdateAsync(int id,PaymentMethodUpdateDto dto);

        public Task<ServiceResult<bool>> ActivateAsync(int id);

        public Task<ServiceResult<bool>> DeactivateAsync(int id);
    }
}
