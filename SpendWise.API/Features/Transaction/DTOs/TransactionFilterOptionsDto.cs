namespace SpendWise.API.Features.Transaction.DTOs
{
    public class TransactionFilterOptionsDto
    {
        public IEnumerable<UserCategoryFilterOptionDto> Categories { get; set; }
            = new List<UserCategoryFilterOptionDto>();

        public IEnumerable<PaymentMethodFilterOptionDto> PaymentMethods { get; set; }
            = new List<PaymentMethodFilterOptionDto>();
    }

    public class UserCategoryFilterOptionDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TypeId { get; set; }

        public bool IsActive { get; set; }
    }

    public class PaymentMethodFilterOptionDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}