namespace TravelApp.Application.DTOs;

public class ItineraryDayHotelDto
{
    public int HotelId { get; set; }
    public HotelDto Hotel { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsRecommended { get; set; }
}

public class CreateItineraryDayHotelRequest
{
    public int HotelId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsRecommended { get; set; } = false;
}
