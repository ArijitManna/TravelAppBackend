namespace TravelApp.Domain.Entities;

public class ItineraryDayActivity
{
    public int Id { get; set; }
    public int ItineraryDayId { get; set; }
    public int ActivityId { get; set; }
    public bool IsRecommended { get; set; } = true;
    public bool IsIncluded { get; set; } = false;
    public int DisplayOrder { get; set; }
    
    // Navigation properties
    public ItineraryDay ItineraryDay { get; set; } = null!;
    public Activity Activity { get; set; } = null!;
}
