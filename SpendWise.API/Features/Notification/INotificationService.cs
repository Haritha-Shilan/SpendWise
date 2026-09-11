using SpendWise.API.Features.Notification.DTOs;

namespace SpendWise.API.Features.Notification
{
    public interface INotificationService
    {
        public Task<ServiceResult<IEnumerable<NotificationResponseDto>>> GetAllAsync();

        public Task<ServiceResult<NotificationResponseDto>> GetByIdAsync(int id);

        public Task<ServiceResult<NotificationResponseDto>> CreateAsync(NotificationCreateDto dto);

        public Task<ServiceResult<bool>> MarkAsReadAsync(int id);

        public Task<ServiceResult<bool>> MarkAllAsReadAsync();

    }
}
