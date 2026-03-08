using TravelApp.Application.DTOs;

namespace TravelApp.Application.Interfaces;

public interface IHotelService
{
    Task<List<HotelDto>> GetAllAsync();
    Task<HotelDto?> GetByIdAsync(int id);
    Task<HotelDto> CreateAsync(CreateHotelRequest request);
    Task<HotelDto> UpdateAsync(int id, UpdateHotelRequest request);
    Task<bool> DeleteAsync(int id);
}
