namespace TravelApp.Application.DTOs;

public class DashboardStatsDto
{
    public StatCardDto TotalDestinations { get; set; } = new();
    public StatCardDto TotalPackages { get; set; } = new();
    public StatCardDto ActiveBookings { get; set; } = new();
    public RevenueStatDto Revenue { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class StatCardDto
{
    public int Value { get; set; }
    public decimal PercentageChange { get; set; }
}

public class RevenueStatDto
{
    public decimal Value { get; set; }
    public decimal PercentageChange { get; set; }
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty; // "booking", "package", "destination"
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Icon { get; set; } = string.Empty;
}
