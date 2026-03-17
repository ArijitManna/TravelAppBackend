using TravelApp.Application.DTOs;

namespace TravelApp.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
