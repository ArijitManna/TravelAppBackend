namespace TravelApp.Domain.Entities;

public class ItineraryDayHotel
{
    public int Id { get; set; }
    public int ItineraryDayId { get; set; }
    public int HotelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsRecommended { get; set; } = false;
    
    // Navigation properties
    public ItineraryDay ItineraryDay { get; set; } = null!;
    public Hotel Hotel { get; set; } = null!;
}
