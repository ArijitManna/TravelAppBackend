namespace TravelApp.Application.DTOs;

public class ItineraryDayDto
{
    public int Id { get; set; }
    public int DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsBreakfastIncluded { get; set; }
    public bool IsLunchIncluded { get; set; }
    public bool IsDinnerIncluded { get; set; }
    public bool IsLeisureDay { get; set; }
    public List<ItineraryDayImageDto> Images { get; set; } = new();
    public List<ItineraryDayHotelDto> Hotels { get; set; } = new();
    public List<ItineraryDayActivityDto> Activities { get; set; } = new();
    public List<TransferDto> Transfers { get; set; } = new();
}
