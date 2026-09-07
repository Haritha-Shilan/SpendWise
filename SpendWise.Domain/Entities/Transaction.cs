namespace SpendWise.Domain.Entities
{
    public class Transaction : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }

        public int UserCategoryId { get; set; }

        public UserCategory? UserCategory { get; set; }

        public int PaymentMethodId { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public TransactionAttachment? TransactionAttachment { get; set; }
    }
}
