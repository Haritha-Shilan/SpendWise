public class AdminDashboardResponseDto
{
    public int TotalUsers { get; set; }
    public int NewThisMonth { get; set; }
    public int ActiveCategories { get; set; }

    public NotificationSummaryDto? LatestNotification { get; set; }

    public IEnumerable<RecentRegistrationDto> RecentRegistrations { get; set; }
        = new List<RecentRegistrationDto>();
}

public class NotificationSummaryDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}

public class RecentRegistrationDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}