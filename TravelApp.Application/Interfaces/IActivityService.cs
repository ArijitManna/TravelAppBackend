using TravelApp.Application.DTOs;

namespace TravelApp.Application.Interfaces;

public interface IActivityService
{
    Task<List<ActivityDto>> GetAllAsync();
    Task<ActivityDto?> GetByIdAsync(int id);
    Task<ActivityDto> CreateAsync(CreateActivityRequest request);
    Task<ActivityDto> UpdateAsync(int id, UpdateActivityRequest request);
    Task<bool> DeleteAsync(int id);
}
