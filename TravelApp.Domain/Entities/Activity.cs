namespace TravelApp.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Duration { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<ActivityImage> Images { get; set; } = new List<ActivityImage>();
    public ICollection<ItineraryDayActivity> ItineraryDayActivities { get; set; } = new List<ItineraryDayActivity>();
}
