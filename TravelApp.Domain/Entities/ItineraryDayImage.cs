namespace TravelApp.Domain.Entities;

public class ItineraryDayImage
{
    public int Id { get; set; }
    public int ItineraryDayId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    // Navigation property
    public ItineraryDay ItineraryDay { get; set; } = null!;
}
