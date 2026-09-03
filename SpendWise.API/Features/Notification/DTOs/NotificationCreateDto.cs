namespace SpendWise.API.Features.Notification.DTOs
{
    public class NotificationCreateDto
    {
        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;
    }
}
