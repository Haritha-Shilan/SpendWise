namespace SpendWise.API.Features.PaymentMethod.DTOs
{
    public class PaymentMethodResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
