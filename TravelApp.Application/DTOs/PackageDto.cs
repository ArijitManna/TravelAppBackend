namespace TravelApp.Application.DTOs;

public class PackageDto
{
    public int Id { get; set; }
    public int DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? OriginalPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int? MaxPeople { get; set; }
    public string? Category { get; set; }
    public string? ThemeTags { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PackageImageDto> Images { get; set; } = new();
    public List<PackageInclusionDto> Inclusions { get; set; } = new();
    public List<PackageHighlightDto> Highlights { get; set; } = new();
    public List<ItineraryDayDto> Itinerary { get; set; } = new();
}
