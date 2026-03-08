namespace TravelApp.Domain.Entities;

public class ActivityImage
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    // Navigation property
    public Activity Activity { get; set; } = null!;
}
