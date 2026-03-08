namespace TravelApp.Application.DTOs;

public class ItineraryDayActivityDto
{
    public int ActivityId { get; set; }
    public ActivityDto Activity { get; set; } = null!;
    public bool IsRecommended { get; set; }
    public bool IsIncluded { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateItineraryDayActivityRequest
{
    public int ActivityId { get; set; }
    public bool IsRecommended { get; set; } = true;
    public bool IsIncluded { get; set; } = false;
    public int DisplayOrder { get; set; }
}
