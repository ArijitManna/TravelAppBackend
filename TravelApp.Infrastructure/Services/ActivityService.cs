using Microsoft.EntityFrameworkCore;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;
using TravelApp.Domain.Entities;
using TravelApp.Infrastructure.Data;

namespace TravelApp.Infrastructure.Services;

public class ActivityService : IActivityService
{
    private readonly TravelAppDbContext _context;

    public ActivityService(TravelAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityDto>> GetAllAsync()
    {
        var activities = await _context.Activities
            .Include(a => a.Images)
            .Where(a => a.IsActive)
            .ToListAsync();

        return activities.Select(MapToDto).ToList();
    }

    public async Task<ActivityDto?> GetByIdAsync(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        return activity == null ? null : MapToDto(activity);
    }

    public async Task<ActivityDto> CreateAsync(CreateActivityRequest request)
    {
        var activity = new Activity
        {
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Duration = request.Duration,
            Location = request.Location,
            IsActive = true
        };

        // Add images
        foreach (var imageUrl in request.ImageUrls)
        {
            activity.Images.Add(new ActivityImage { ImageUrl = imageUrl });
        }

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        // Reload with includes
        return (await GetByIdAsync(activity.Id))!;
    }

    public async Task<ActivityDto> UpdateAsync(int id, UpdateActivityRequest request)
    {
        var activity = await _context.Activities
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null)
        {
            throw new Exception($"Activity with ID {id} not found");
        }

        // Update activity
        activity.Name = request.Name;
        activity.Description = request.Description;
        activity.ImageUrl = request.ImageUrl;
        activity.Price = request.Price;
        activity.Duration = request.Duration;
        activity.Location = request.Location;
        activity.IsActive = request.IsActive;

        // Update images - remove old, add new
        _context.ActivityImages.RemoveRange(activity.Images);
        foreach (var imageUrl in request.ImageUrls)
        {
            activity.Images.Add(new ActivityImage { ImageUrl = imageUrl, ActivityId = activity.Id });
        }

        await _context.SaveChangesAsync();

        // Reload with includes
        return (await GetByIdAsync(activity.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
        {
            return false;
        }

        // Soft delete by setting IsActive to false
        activity.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    private static ActivityDto MapToDto(Activity activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            Name = activity.Name,
            Description = activity.Description,
            ImageUrl = activity.ImageUrl,
            Price = activity.Price,
            Duration = activity.Duration,
            Location = activity.Location,
            Images = activity.Images.Select(i => new ActivityImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl
            }).ToList()
        };
    }
}
