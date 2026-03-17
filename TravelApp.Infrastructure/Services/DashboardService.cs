using Microsoft.EntityFrameworkCore;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;
using TravelApp.Infrastructure.Data;

namespace TravelApp.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly TravelAppDbContext _context;

    public DashboardService(TravelAppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var now = DateTime.UtcNow;
        var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startOfLastMonth = startOfCurrentMonth.AddMonths(-1);

        // Get current month counts
        var totalDestinations = await _context.Destinations.CountAsync();
        var totalPackages = await _context.Packages.CountAsync();
        var activeBookings = await _context.Bookings
            .CountAsync(b => b.Status != "Cancelled" && b.Status != "Completed");
        var currentMonthRevenue = await _context.Bookings
            .Where(b => b.CreatedAt >= startOfCurrentMonth && 
                       (b.Status == "Confirmed" || b.Status == "Completed"))
            .SumAsync(b => (decimal?)b.TotalAmount) ?? 0;

        // Get last month counts for percentage calculations
        var lastMonthDestinations = await _context.Destinations
            .CountAsync(d => d.CreatedAt < startOfCurrentMonth);
        var lastMonthPackages = await _context.Packages
            .CountAsync(p => p.CreatedAt < startOfCurrentMonth);
        var lastMonthActiveBookings = await _context.Bookings
            .CountAsync(b => b.CreatedAt < startOfCurrentMonth && 
                           b.Status != "Cancelled" && b.Status != "Completed");
        var lastMonthRevenue = await _context.Bookings
            .Where(b => b.CreatedAt >= startOfLastMonth && 
                       b.CreatedAt < startOfCurrentMonth &&
                       (b.Status == "Confirmed" || b.Status == "Completed"))
            .SumAsync(b => (decimal?)b.TotalAmount) ?? 0;

        // Get recent activities
        var recentActivities = new List<RecentActivityDto>();

        // Recent bookings
        var recentBookings = await _context.Bookings
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .Include(b => b.User)
            .Include(b => b.Package)
            .ToListAsync();

        foreach (var booking in recentBookings)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Type = "booking",
                Description = $"New booking for {booking.Package.Title} by {booking.User.FullName}",
                Timestamp = booking.CreatedAt,
                Icon = "booking"
            });
        }

        // Recent packages
        var recentPackages = await _context.Packages
            .OrderByDescending(p => p.CreatedAt)
            .Take(3)
            .ToListAsync();

        foreach (var package in recentPackages)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Type = "package",
                Description = $"New package created: {package.Title}",
                Timestamp = package.CreatedAt,
                Icon = "package"
            });
        }

        // Recent destinations
        var recentDestinations = await _context.Destinations
            .OrderByDescending(d => d.CreatedAt)
            .Take(2)
            .ToListAsync();

        foreach (var destination in recentDestinations)
        {
            recentActivities.Add(new RecentActivityDto
            {
                Type = "destination",
                Description = $"New destination added: {destination.Name}, {destination.Country}",
                Timestamp = destination.CreatedAt,
                Icon = "destination"
            });
        }

        // Sort all activities by timestamp and take top 10
        recentActivities = recentActivities
            .OrderByDescending(a => a.Timestamp)
            .Take(10)
            .ToList();

        return new DashboardStatsDto
        {
            TotalDestinations = new StatCardDto
            {
                Value = totalDestinations,
                PercentageChange = CalculatePercentageChange(totalDestinations, lastMonthDestinations)
            },
            TotalPackages = new StatCardDto
            {
                Value = totalPackages,
                PercentageChange = CalculatePercentageChange(totalPackages, lastMonthPackages)
            },
            ActiveBookings = new StatCardDto
            {
                Value = activeBookings,
                PercentageChange = CalculatePercentageChange(activeBookings, lastMonthActiveBookings)
            },
            Revenue = new RevenueStatDto
            {
                Value = currentMonthRevenue,
                PercentageChange = CalculatePercentageChange(currentMonthRevenue, lastMonthRevenue)
            },
            RecentActivities = recentActivities
        };
    }

    private decimal CalculatePercentageChange(int current, int previous)
    {
        if (previous == 0)
            return current > 0 ? 100 : 0;
        
        return Math.Round(((decimal)(current - previous) / previous) * 100, 2);
    }

    private decimal CalculatePercentageChange(decimal current, decimal previous)
    {
        if (previous == 0)
            return current > 0 ? 100 : 0;
        
        return Math.Round(((current - previous) / previous) * 100, 2);
    }
}
