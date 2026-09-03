namespace SpendWise.API.Features.PaymentMethod.DTO
{
    public class PaymentMethodResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
