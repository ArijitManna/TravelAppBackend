using Microsoft.EntityFrameworkCore;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;
using TravelApp.Domain.Entities;
using TravelApp.Infrastructure.Data;

namespace TravelApp.Infrastructure.Services;

public class DestinationService : IDestinationService
{
    private readonly TravelAppDbContext _context;

    public DestinationService(TravelAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DestinationDto>> GetAllAsync()
    {
        var destinations = await _context.Destinations
            .Include(d => d.Images)
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();

        return destinations.Select(d => MapToDto(d));
    }

    public async Task<DestinationDto?> GetByIdAsync(int id)
    {
        var destination = await _context.Destinations
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == id);

        return destination == null ? null : MapToDto(destination);
    }

    public async Task<DestinationDto> CreateAsync(CreateDestinationRequest request)
    {
        var destination = new Destination
        {
            Name = request.Name,
            Country = request.Country,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();

        // Add images if provided
        if (request.ImageUrls != null && request.ImageUrls.Any())
        {
            foreach (var imageUrl in request.ImageUrls)
            {
                var image = new DestinationImage
                {
                    DestinationId = destination.Id,
                    ImageUrl = imageUrl
                };
                _context.DestinationImages.Add(image);
            }
            await _context.SaveChangesAsync();
        }

        // Reload with images
        await _context.Entry(destination).Collection(d => d.Images).LoadAsync();

        return MapToDto(destination);
    }

    public async Task<DestinationDto> UpdateAsync(int id, UpdateDestinationRequest request)
    {
        var destination = await _context.Destinations
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == id);
            
        if (destination == null)
        {
            throw new Exception($"Destination with ID {id} not found");
        }

        destination.Name = request.Name;
        destination.Country = request.Country;
        destination.Description = request.Description;
        destination.ImageUrl = request.ImageUrl;
        destination.IsActive = request.IsActive;

        // Update images if provided
        if (request.ImageUrls != null)
        {
            // Remove existing images
            _context.DestinationImages.RemoveRange(destination.Images);

            // Add new images
            foreach (var imageUrl in request.ImageUrls)
            {
                destination.Images.Add(new DestinationImage
                {
                    ImageUrl = imageUrl
                });
            }
        }

        await _context.SaveChangesAsync();

        return MapToDto(destination);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var destination = await _context.Destinations.FindAsync(id);
        if (destination == null)
        {
            return false;
        }

        _context.Destinations.Remove(destination);
        await _context.SaveChangesAsync();

        return true;
    }

    private static DestinationDto MapToDto(Destination destination)
    {
        return new DestinationDto
        {
            Id = destination.Id,
            Name = destination.Name,
            Country = destination.Country,
            Description = destination.Description,
            ImageUrl = destination.ImageUrl,
            Images = destination.Images.Select(i => new DestinationImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl
            }).ToList(),
            IsActive = destination.IsActive,
            CreatedAt = destination.CreatedAt
        };
    }
}
