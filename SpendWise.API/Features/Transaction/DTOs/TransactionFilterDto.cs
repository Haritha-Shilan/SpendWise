namespace SpendWise.API.Features.Transaction.DTOs
{
    public class TransactionFilterDto
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? UserCategoryId { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? CategoryTypeId { get; set; }

        public string? FilterText { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
