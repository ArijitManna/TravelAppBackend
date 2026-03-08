using Microsoft.EntityFrameworkCore;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;
using TravelApp.Domain.Entities;
using TravelApp.Infrastructure.Data;

namespace TravelApp.Infrastructure.Services;

public class HotelService : IHotelService
{
    private readonly TravelAppDbContext _context;

    public HotelService(TravelAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HotelDto>> GetAllAsync()
    {
        var hotels = await _context.Hotels
            .Include(h => h.Images)
            .Where(h => h.IsActive)
            .ToListAsync();

        return hotels.Select(MapToDto).ToList();
    }

    public async Task<HotelDto?> GetByIdAsync(int id)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == id);

        return hotel == null ? null : MapToDto(hotel);
    }

    public async Task<HotelDto> CreateAsync(CreateHotelRequest request)
    {
        var hotel = new Hotel
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            Rating = request.Rating,
            ImageUrl = request.ImageUrl,
            CheckInTime = request.CheckInTime,
            CheckOutTime = request.CheckOutTime,
            IsActive = true
        };

        // Add images
        foreach (var imageUrl in request.ImageUrls)
        {
            hotel.Images.Add(new HotelImage { ImageUrl = imageUrl });
        }

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        // Reload with includes
        return (await GetByIdAsync(hotel.Id))!;
    }

    public async Task<HotelDto> UpdateAsync(int id, UpdateHotelRequest request)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            throw new Exception($"Hotel with ID {id} not found");
        }

        // Update hotel
        hotel.Name = request.Name;
        hotel.Description = request.Description;
        hotel.Address = request.Address;
        hotel.Rating = request.Rating;
        hotel.ImageUrl = request.ImageUrl;
        hotel.CheckInTime = request.CheckInTime;
        hotel.CheckOutTime = request.CheckOutTime;
        hotel.IsActive = request.IsActive;

        // Update images - remove old, add new
        _context.HotelImages.RemoveRange(hotel.Images);
        foreach (var imageUrl in request.ImageUrls)
        {
            hotel.Images.Add(new HotelImage { ImageUrl = imageUrl, HotelId = hotel.Id });
        }

        await _context.SaveChangesAsync();

        // Reload with includes
        return (await GetByIdAsync(hotel.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return false;
        }

        // Soft delete by setting IsActive to false
        hotel.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    private static HotelDto MapToDto(Hotel hotel)
    {
        return new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            Address = hotel.Address,
            Rating = hotel.Rating,
            ImageUrl = hotel.ImageUrl,
            CheckInTime = hotel.CheckInTime,
            CheckOutTime = hotel.CheckOutTime,
            Images = hotel.Images.Select(i => new HotelImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl
            }).ToList()
        };
    }
}
