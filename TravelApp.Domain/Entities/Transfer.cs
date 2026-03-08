namespace TravelApp.Domain.Entities;

public class Transfer
{
    public int Id { get; set; }
    public int ItineraryDayId { get; set; }
    public string VehicleType { get; set; } = string.Empty; // SUV, Sedan, Bus, etc.
    public string PickupLocation { get; set; } = string.Empty;
    public string DropLocation { get; set; } = string.Empty;
    public string? PickupTime { get; set; }
    public bool IsPrivate { get; set; } = true;
    
    // Navigation property
    public ItineraryDay ItineraryDay { get; set; } = null!;
}
