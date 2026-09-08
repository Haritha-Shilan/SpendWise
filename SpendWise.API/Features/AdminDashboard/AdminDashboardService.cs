using CategoryMasterEntity = SpendWise.Domain.Entities.CategoryMaster;
using NotificationEntity = SpendWise.Domain.Entities.Notification;

namespace SpendWise.API.Features.AdminDashboard;
public class AdminDashboardService : IAdminDashboardService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<CategoryMasterEntity> _categoryRepository;
    private readonly IRepository<NotificationEntity> _notificationRepository;
    private readonly IMapper _mapper;

    public AdminDashboardService(
        UserManager<ApplicationUser> userManager,
        IRepository<CategoryMasterEntity> categoryRepository,
        IRepository<NotificationEntity> notificationRepository,
        IMapper mapper)
    {
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<AdminDashboardResponseDto>> GetDashboard()
    {
        var totalUsers = await _userManager.Users.CountAsync();

        var now = DateTime.UtcNow;

        var startOfMonth = new DateTime(now.Year,now.Month,1);

        var startOfNextMonth = startOfMonth.AddMonths(1);

        var newThisMonth = await _userManager.Users
            .CountAsync(x =>
                x.CreatedAt >= startOfMonth &&
                x.CreatedAt < startOfNextMonth);

        var activeCategories = await _categoryRepository
        .GetAllAsync(new QueryOptions<CategoryMasterEntity>
        {
            Filters =
            {
                x => x.IsActive
            }
        });

        var activeCategoryCount = activeCategories.Count();

        var latestNotification = await GetLatestNotification();
        var recentRegistrations = await GetRecentRegistrations();

        return new ServiceResult<AdminDashboardResponseDto>
        {
            Status = ServiceResultStatus.Success,
            Data = new AdminDashboardResponseDto
            {
                TotalUsers = totalUsers,
                NewThisMonth = newThisMonth,
                ActiveCategories = activeCategoryCount,
                LatestNotification = latestNotification,
                RecentRegistrations = recentRegistrations
            }
        };
    }

    private async Task<List<RecentRegistrationDto>> GetRecentRegistrations()
    {
        var recentUsers = await _userManager.Users
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToListAsync();

        return _mapper.Map<List<RecentRegistrationDto>>(recentUsers);
    }

    private async Task<NotificationSummaryDto?> GetLatestNotification()
    {
        var notifications = await _notificationRepository
            .GetAllAsync(new QueryOptions<NotificationEntity>
            {
                OrderBy = x => x.CreatedAt,
                OrderDescending = true
            });
        var notification = notifications.FirstOrDefault();


        return _mapper.Map<NotificationSummaryDto?>(notification);
    }
}