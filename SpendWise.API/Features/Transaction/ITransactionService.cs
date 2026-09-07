namespace SpendWise.API.Features.Transaction
{
    public interface ITransactionService
    {
        public Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllAsync();

        public Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllByFilterAsync(TransactionFilterDto dto);

        public Task<ServiceResult<TransactionResponseDto>> GetByIdAsync(int id);

        public Task<ServiceResult<TransactionResponseDto>> CreateAsync(TransactionCreateDto dto);

        public Task<ServiceResult<bool>> UpdateAsync(int id,TransactionUpdateDto dto);

        public Task<ServiceResult<bool>> DeleteAsync(int id);

        public Task<ServiceResult<TransactionAttachment>> GetAttachmentAsync(int id);
    }
}
