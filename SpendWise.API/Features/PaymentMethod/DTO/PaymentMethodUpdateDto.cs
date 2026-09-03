namespace SpendWise.API.Features.PaymentMethod.DTO
{
    public class PaymentMethodUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
