using NotificationEntity = SpendWise.Domain.Entities.Notification;
namespace SpendWise.API.Features.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<NotificationEntity> _repository;
        private readonly IMapper _mapper;

        public NotificationService(IRepository<NotificationEntity> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<NotificationResponseDto>> CreateAsync(NotificationCreateDto dto)
        {
            dto.Message=dto.Message.Trim();

            var notification= _mapper.Map<NotificationEntity>(dto);

            notification.IsRead = false;
            notification.CreatedAt = DateTime.UtcNow;
            notification.ReadAt = null;

            await _repository.AddAsync(notification);

            return new ServiceResult<NotificationResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<NotificationResponseDto>(notification)
            };
        }

        public async Task<ServiceResult<IEnumerable<NotificationResponseDto>>> GetAllAsync()
        {
            var options = new QueryOptions<NotificationEntity>
            {
                OrderBy = x => x.CreatedAt,
                OrderDescending = true
            };

            var notifications = await _repository.GetAllAsync(options);

            return new ServiceResult<IEnumerable<NotificationResponseDto>>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map< IEnumerable<NotificationResponseDto>>(notifications)
            };
        }

        public async Task<ServiceResult<NotificationResponseDto>> GetByIdAsync(int id)
        {
            var notification = await _repository.GetByIdAsync(id);

            if (notification == null)
            {
                return new ServiceResult<NotificationResponseDto>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Notification not found."
                };
            }

            return new ServiceResult<NotificationResponseDto>
            {
                Status = ServiceResultStatus.Success,
                Data = _mapper.Map<NotificationResponseDto>(notification)
            };
        }

        public async Task<ServiceResult<bool>> MarkAsReadAsync(int id)
        {
            var notification = await _repository.GetByIdAsync(id);

            if(notification == null)
            {
                return new ServiceResult<bool>
                {
                    Status = ServiceResultStatus.NotFound,
                    Error = "Notification not found."
                };
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                await _repository.UpdateAsync(notification);
            }

            return new ServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Data=true
            };
        }
    }
}
