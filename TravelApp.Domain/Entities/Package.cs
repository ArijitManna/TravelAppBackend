namespace TravelApp.Domain.Entities;

public class Package
{
    public int Id { get; set; }
    public int DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? OriginalPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int? MaxPeople { get; set; }
    public string? Category { get; set; }
    public string? ThemeTags { get; set; } // JSON array: ["Romantic", "Honeymoon", "Adventure"]
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Destination Destination { get; set; } = null!;
    public ICollection<PackageImage> PackageImages { get; set; } = new List<PackageImage>();
    public ICollection<PackageInclusion> Inclusions { get; set; } = new List<PackageInclusion>();
    public ICollection<PackageHighlight> Highlights { get; set; } = new List<PackageHighlight>();
    public ICollection<ItineraryDay> ItineraryDays { get; set; } = new List<ItineraryDay>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
