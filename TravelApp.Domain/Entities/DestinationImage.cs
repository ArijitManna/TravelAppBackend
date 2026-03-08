namespace TravelApp.Domain.Entities;

public class DestinationImage
{
    public int Id { get; set; }
    public int DestinationId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    // Navigation property
    public Destination Destination { get; set; } = null!;
}
