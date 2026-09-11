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
        var users = await _userManager.GetUsersInRoleAsync("User");

        var totalUsers = users.Count;

        var activeUsers = users.Count(x => x.IsActive);

        var inactiveUsers = users.Count(x => !x.IsActive);

        var now = DateTime.UtcNow;

        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        var startOfNextMonth = startOfMonth.AddMonths(1);

        var newThisMonth = users.Count(x =>
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

        var UnreadNotificationCount = await GetUnreadNotificationCount();
        var recentRegistrations = await GetRecentRegistrations();

        return new ServiceResult<AdminDashboardResponseDto>
        {
            Status = ServiceResultStatus.Success,
            Data = new AdminDashboardResponseDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                NewThisMonth = newThisMonth,
                ActiveCategories = activeCategoryCount,
                UnreadNotificationCount = await GetUnreadNotificationCount(),
                RecentRegistrations = await GetRecentRegistrations()
            }
        };
    }

    private async Task<List<RecentRegistrationDto>> GetRecentRegistrations()
    {
        var users = await _userManager.GetUsersInRoleAsync("User");

        var recentUsers = users
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToList();

        return _mapper.Map<List<RecentRegistrationDto>>(recentUsers);
    }

    private async Task<int> GetUnreadNotificationCount()
    {
        var options = new QueryOptions<NotificationEntity>();
        options.Filters.Add(x => !x.IsRead);

        var unreadNotifications = await _notificationRepository.GetAllAsync(options);
            
        return unreadNotifications.Count();
    }
}