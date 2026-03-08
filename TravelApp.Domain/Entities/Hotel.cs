namespace TravelApp.Domain.Entities;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();
    public ICollection<ItineraryDayHotel> ItineraryDayHotels { get; set; } = new List<ItineraryDayHotel>();
}
