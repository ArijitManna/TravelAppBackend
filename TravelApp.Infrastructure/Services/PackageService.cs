using Microsoft.EntityFrameworkCore;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;
using TravelApp.Domain.Entities;
using TravelApp.Infrastructure.Data;

namespace TravelApp.Infrastructure.Services;

public class PackageService : IPackageService
{
    private readonly TravelAppDbContext _context;

    public PackageService(TravelAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PackageDto>> GetAllAsync(PackageFilterRequest? filter = null)
    {
        var query = _context.Packages
            .Include(p => p.Destination)
            .Include(p => p.PackageImages)
            .Include(p => p.ItineraryDays)
            .Where(p => p.IsActive)
            .AsQueryable();

        // Apply filters
        if (filter != null)
        {
            if (filter.DestinationId.HasValue)
                query = query.Where(p => p.DestinationId == filter.DestinationId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.BasePrice >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.BasePrice <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(p => p.Category == filter.Category);

            if (filter.IsFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);
        }

        var packages = await query
            .OrderByDescending(p => p.IsFeatured)
            .ThenBy(p => p.Title)
            .ToListAsync();

        return packages.Select(MapToDto);
    }

    public async Task<PackageDto?> GetByIdAsync(int id)
    {
        var package = await _context.Packages
            .Include(p => p.Destination)
            .Include(p => p.PackageImages)
            .Include(p => p.Inclusions)
            .Include(p => p.Highlights)
            .Include(p => p.ItineraryDays.OrderBy(i => i.DayNumber))
                .ThenInclude(i => i.Images)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Hotels)
                    .ThenInclude(h => h.Hotel)
                        .ThenInclude(h => h.Images)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Activities)
                    .ThenInclude(a => a.Activity)
                        .ThenInclude(a => a.Images)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Transfers)
            .FirstOrDefaultAsync(p => p.Id == id);

        return package == null ? null : MapToDto(package);
    }

    public async Task<PackageDto> CreateAsync(CreatePackageRequest request)
    {
        // Verify destination exists
        var destinationExists = await _context.Destinations.AnyAsync(d => d.Id == request.DestinationId);
        if (!destinationExists)
        {
            throw new Exception($"Destination with ID {request.DestinationId} not found");
        }

        var package = new Package
        {
            DestinationId = request.DestinationId,
            Title = request.Title,
            Description = request.Description,
            DurationDays = request.DurationDays,
            BasePrice = request.BasePrice,
            OriginalPrice = request.OriginalPrice,
            DiscountAmount = request.DiscountAmount,
            ThemeTags = request.ThemeTags,
            MaxPeople = request.MaxPeople,
            Category = request.Category,
            IsFeatured = request.IsFeatured,
            IsActive = true,
            StartDate = request.StartDate.HasValue ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc) : null,
            EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null,
            CreatedAt = DateTime.UtcNow
        };

        // Add images
        foreach (var imageUrl in request.ImageUrls)
        {
            package.PackageImages.Add(new PackageImage { ImageUrl = imageUrl });
        }

        // Add inclusions
        if (request.Inclusions != null)
        {
            foreach (var inclusion in request.Inclusions)
            {
                package.Inclusions.Add(new PackageInclusion
                {
                    InclusionType = inclusion.InclusionType,
                    IconName = inclusion.IconName,
                    IsIncluded = inclusion.IsIncluded
                });
            }
        }

        // Add highlights
        if (request.Highlights != null)
        {
            foreach (var highlight in request.Highlights)
            {
                package.Highlights.Add(new PackageHighlight
                {
                    Highlight = highlight.Highlight,
                    DisplayOrder = highlight.DisplayOrder
                });
            }
        }

        // Add itinerary
        foreach (var day in request.Itinerary)
        {
            var itineraryDay = new ItineraryDay
            {
                DayNumber = day.DayNumber,
                Title = day.Title,
                Description = day.Description,
                IsBreakfastIncluded = day.IsBreakfastIncluded,
                IsLunchIncluded = day.IsLunchIncluded,
                IsDinnerIncluded = day.IsDinnerIncluded,
                IsLeisureDay = day.IsLeisureDay
            };

            // Add images for this itinerary day
            foreach (var imageUrl in day.ImageUrls)
            {
                itineraryDay.Images.Add(new ItineraryDayImage { ImageUrl = imageUrl });
            }

            // Add hotels for this day
            if (day.Hotels != null)
            {
                foreach (var hotel in day.Hotels)
                {
                    itineraryDay.Hotels.Add(new ItineraryDayHotel
                    {
                        HotelId = hotel.HotelId,
                        DisplayOrder = hotel.DisplayOrder,
                        IsRecommended = hotel.IsRecommended
                    });
                }
            }

            // Add activities for this day
            if (day.Activities != null)
            {
                foreach (var activity in day.Activities)
                {
                    itineraryDay.Activities.Add(new ItineraryDayActivity
                    {
                        ActivityId = activity.ActivityId,
                        IsRecommended = activity.IsRecommended,
                        IsIncluded = activity.IsIncluded,
                        DisplayOrder = activity.DisplayOrder
                    });
                }
            }

            // Add transfers for this day
            if (day.Transfers != null)
            {
                foreach (var transfer in day.Transfers)
                {
                    itineraryDay.Transfers.Add(new Transfer
                    {
                        VehicleType = transfer.VehicleType,
                        PickupLocation = transfer.PickupLocation,
                        DropLocation = transfer.DropLocation,
                        PickupTime = transfer.PickupTime,
                        IsPrivate = transfer.IsPrivate
                    });
                }
            }

            package.ItineraryDays.Add(itineraryDay);
        }

        _context.Packages.Add(package);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Get detailed error information including inner exceptions
            var errorMessage = ex.Message;
            var innerException = ex.InnerException;
            var detailedError = errorMessage;
            
            while (innerException != null)
            {
                detailedError += $" | Inner Exception: {innerException.Message}";
                innerException = innerException.InnerException;
            }
            
            throw new Exception($"Database save failed: {detailedError}", ex);
        }

        // Reload with includes
        return (await GetByIdAsync(package.Id))!;
    }

    public async Task<PackageDto> UpdateAsync(int id, UpdatePackageRequest request)
    {
        var package = await _context.Packages
            .Include(p => p.PackageImages)
            .Include(p => p.Inclusions)
            .Include(p => p.Highlights)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Hotels)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Activities)
            .Include(p => p.ItineraryDays)
                .ThenInclude(i => i.Transfers)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (package == null)
        {
            throw new Exception($"Package with ID {id} not found");
        }

        // Verify destination exists
        var destinationExists = await _context.Destinations.AnyAsync(d => d.Id == request.DestinationId);
        if (!destinationExists)
        {
            throw new Exception($"Destination with ID {request.DestinationId} not found");
        }

        // Update package
        package.DestinationId = request.DestinationId;
        package.Title = request.Title;
        package.Description = request.Description;
        package.DurationDays = request.DurationDays;
        package.BasePrice = request.BasePrice;
        package.OriginalPrice = request.OriginalPrice;
        package.DiscountAmount = request.DiscountAmount;
        package.ThemeTags = request.ThemeTags;
        package.MaxPeople = request.MaxPeople;
        package.Category = request.Category;
        package.IsFeatured = request.IsFeatured;
        package.IsActive = request.IsActive;
        package.StartDate = request.StartDate.HasValue ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc) : null;
        package.EndDate = request.EndDate.HasValue ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc) : null;

        // Update images - remove old, add new
        _context.PackageImages.RemoveRange(package.PackageImages);
        foreach (var imageUrl in request.ImageUrls)
        {
            package.PackageImages.Add(new PackageImage { ImageUrl = imageUrl, PackageId = package.Id });
        }

        // Update inclusions - remove old, add new
        _context.PackageInclusions.RemoveRange(package.Inclusions);
        if (request.Inclusions != null)
        {
            foreach (var inclusion in request.Inclusions)
            {
                package.Inclusions.Add(new PackageInclusion
                {
                    PackageId = package.Id,
                    InclusionType = inclusion.InclusionType,
                    IconName = inclusion.IconName,
                    IsIncluded = inclusion.IsIncluded
                });
            }
        }

        // Update highlights - remove old, add new
        _context.PackageHighlights.RemoveRange(package.Highlights);
        if (request.Highlights != null)
        {
            foreach (var highlight in request.Highlights)
            {
                package.Highlights.Add(new PackageHighlight
                {
                    PackageId = package.Id,
                    Highlight = highlight.Highlight,
                    DisplayOrder = highlight.DisplayOrder
                });
            }
        }

        // Update itinerary - remove old, add new
        _context.ItineraryDays.RemoveRange(package.ItineraryDays);
        foreach (var day in request.Itinerary)
        {
            var itineraryDay = new ItineraryDay
            {
                PackageId = package.Id,
                DayNumber = day.DayNumber,
                Title = day.Title,
                Description = day.Description,
                IsBreakfastIncluded = day.IsBreakfastIncluded,
                IsLunchIncluded = day.IsLunchIncluded,
                IsDinnerIncluded = day.IsDinnerIncluded,
                IsLeisureDay = day.IsLeisureDay
            };

            // Add images for this itinerary day
            foreach (var imageUrl in day.ImageUrls)
            {
                itineraryDay.Images.Add(new ItineraryDayImage { ImageUrl = imageUrl });
            }

            // Add hotels for this day
            if (day.Hotels != null)
            {
                foreach (var hotel in day.Hotels)
                {
                    itineraryDay.Hotels.Add(new ItineraryDayHotel
                    {
                        HotelId = hotel.HotelId,
                        DisplayOrder = hotel.DisplayOrder,
                        IsRecommended = hotel.IsRecommended
                    });
                }
            }

            // Add activities for this day
            if (day.Activities != null)
            {
                foreach (var activity in day.Activities)
                {
                    itineraryDay.Activities.Add(new ItineraryDayActivity
                    {
                        ActivityId = activity.ActivityId,
                        IsRecommended = activity.IsRecommended,
                        IsIncluded = activity.IsIncluded,
                        DisplayOrder = activity.DisplayOrder
                    });
                }
            }

            // Add transfers for this day
            if (day.Transfers != null)
            {
                foreach (var transfer in day.Transfers)
                {
                    itineraryDay.Transfers.Add(new Transfer
                    {
                        VehicleType = transfer.VehicleType,
                        PickupLocation = transfer.PickupLocation,
                        DropLocation = transfer.DropLocation,
                        PickupTime = transfer.PickupTime,
                        IsPrivate = transfer.IsPrivate
                    });
                }
            }

            package.ItineraryDays.Add(itineraryDay);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Get detailed error information including inner exceptions
            var errorMessage = ex.Message;
            var innerException = ex.InnerException;
            var detailedError = errorMessage;
            
            while (innerException != null)
            {
                detailedError += $" | Inner Exception: {innerException.Message}";
                innerException = innerException.InnerException;
            }
            
            throw new Exception($"Database update failed: {detailedError}", ex);
        }

        // Reload with includes
        return (await GetByIdAsync(package.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null)
        {
            return false;
        }

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();

        return true;
    }

    private static PackageDto MapToDto(Package package)
    {
        return new PackageDto
        {
            Id = package.Id,
            DestinationId = package.DestinationId,
            DestinationName = package.Destination?.Name ?? "",
            Title = package.Title,
            Description = package.Description,
            DurationDays = package.DurationDays,
            BasePrice = package.BasePrice,
            OriginalPrice = package.OriginalPrice,
            DiscountAmount = package.DiscountAmount,
            ThemeTags = package.ThemeTags,
            MaxPeople = package.MaxPeople,
            Category = package.Category,
            IsFeatured = package.IsFeatured,
            IsActive = package.IsActive,
            StartDate = package.StartDate,
            EndDate = package.EndDate,
            CreatedAt = package.CreatedAt,
            Images = package.PackageImages.Select(i => new PackageImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl
            }).ToList(),
            Inclusions = package.Inclusions.Select(inc => new PackageInclusionDto
            {
                Id = inc.Id,
                InclusionType = inc.InclusionType,
                IconName = inc.IconName,
                IsIncluded = inc.IsIncluded
            }).ToList(),
            Highlights = package.Highlights.Select(h => new PackageHighlightDto
            {
                Id = h.Id,
                Highlight = h.Highlight,
                DisplayOrder = h.DisplayOrder
            }).ToList(),
            Itinerary = package.ItineraryDays
                .OrderBy(i => i.DayNumber)
                .Select(i => new ItineraryDayDto
                {
                    Id = i.Id,
                    DayNumber = i.DayNumber,
                    Title = i.Title,
                    Description = i.Description,
                    IsBreakfastIncluded = i.IsBreakfastIncluded,
                    IsLunchIncluded = i.IsLunchIncluded,
                    IsDinnerIncluded = i.IsDinnerIncluded,
                    IsLeisureDay = i.IsLeisureDay,
                    Images = i.Images.Select(img => new ItineraryDayImageDto
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList(),
                    Hotels = i.Hotels.Select(h => new ItineraryDayHotelDto
                    {
                        HotelId = h.HotelId,
                        Hotel = new HotelDto
                        {
                            Id = h.Hotel.Id,
                            Name = h.Hotel.Name,
                            Description = h.Hotel.Description,
                            Address = h.Hotel.Address,
                            Rating = h.Hotel.Rating,
                            ImageUrl = h.Hotel.ImageUrl,
                            CheckInTime = h.Hotel.CheckInTime,
                            CheckOutTime = h.Hotel.CheckOutTime,
                            Images = h.Hotel.Images.Select(img => new HotelImageDto
                            {
                                Id = img.Id,
                                ImageUrl = img.ImageUrl
                            }).ToList()
                        },
                        DisplayOrder = h.DisplayOrder,
                        IsRecommended = h.IsRecommended
                    }).ToList(),
                    Activities = i.Activities.Select(a => new ItineraryDayActivityDto
                    {
                        ActivityId = a.ActivityId,
                        Activity = new ActivityDto
                        {
                            Id = a.Activity.Id,
                            Name = a.Activity.Name,
                            Description = a.Activity.Description,
                            ImageUrl = a.Activity.ImageUrl,
                            Price = a.Activity.Price,
                            Duration = a.Activity.Duration,
                            Location = a.Activity.Location,
                            Images = a.Activity.Images.Select(img => new ActivityImageDto
                            {
                                Id = img.Id,
                                ImageUrl = img.ImageUrl
                            }).ToList()
                        },
                        IsRecommended = a.IsRecommended,
                        IsIncluded = a.IsIncluded,
                        DisplayOrder = a.DisplayOrder
                    }).ToList(),
                    Transfers = i.Transfers.Select(t => new TransferDto
                    {
                        Id = t.Id,
                        VehicleType = t.VehicleType,
                        PickupLocation = t.PickupLocation,
                        DropLocation = t.DropLocation,
                        PickupTime = t.PickupTime,
                        IsPrivate = t.IsPrivate
                    }).ToList()
                }).ToList()
        };
    }
}
