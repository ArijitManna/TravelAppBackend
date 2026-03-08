namespace TravelApp.Application.DTOs;

public class CreatePackageRequest
{
    public int DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePrice { get; set; }
    public decimal? OriginalPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int? MaxPeople { get; set; }
    public string? Category { get; set; }
    public string? ThemeTags { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public List<CreatePackageInclusionRequest> Inclusions { get; set; } = new();
    public List<CreatePackageHighlightRequest> Highlights { get; set; } = new();
    public List<CreateItineraryDayRequest> Itinerary { get; set; } = new();
}

public class CreateItineraryDayRequest
{
    public int DayNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsBreakfastIncluded { get; set; } = false;
    public bool IsLunchIncluded { get; set; } = false;
    public bool IsDinnerIncluded { get; set; } = false;
    public bool IsLeisureDay { get; set; } = false;
    public List<string> ImageUrls { get; set; } = new();
    public List<CreateItineraryDayHotelRequest> Hotels { get; set; } = new();
    public List<CreateItineraryDayActivityRequest> Activities { get; set; } = new();
    public List<CreateTransferRequest> Transfers { get; set; } = new();
}
