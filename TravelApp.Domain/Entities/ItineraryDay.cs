namespace TravelApp.Domain.Entities;

public class ItineraryDay
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public int DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsBreakfastIncluded { get; set; } = false;
    public bool IsLunchIncluded { get; set; } = false;
    public bool IsDinnerIncluded { get; set; } = false;
    public bool IsLeisureDay { get; set; } = false;
    
    // Navigation properties
    public Package Package { get; set; } = null!;
    public ICollection<ItineraryDayImage> Images { get; set; } = new List<ItineraryDayImage>();
    public ICollection<ItineraryDayHotel> Hotels { get; set; } = new List<ItineraryDayHotel>();
    public ICollection<ItineraryDayActivity> Activities { get; set; } = new List<ItineraryDayActivity>();
    public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}
