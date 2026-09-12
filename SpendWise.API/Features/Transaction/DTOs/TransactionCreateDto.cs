namespace SpendWise.API.Features.Transaction.DTOs
{
    public class TransactionCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserCategoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PaymentMethodId { get; set; }

        [Required]
        [Range(0.01,double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; } = string.Empty;

        public IFormFile? Attachment { get; set; }
    }
}
