namespace SpendWise.API.Features.Transaction.DTOs
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }

        public int UserCategoryId { get; set; }

        public string UserCategoryName { get; set; } = string.Empty;

        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; } = string.Empty;

        public int CategoryTypeId { get; set; }

        public string CategoryTypeName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool HasAttachment { get; set; }

        public string? AttachmentFileName { get; set; }
    }
}
