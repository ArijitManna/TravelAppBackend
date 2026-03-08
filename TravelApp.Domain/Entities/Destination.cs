namespace TravelApp.Domain.Entities;

public class Destination
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<DestinationImage> Images { get; set; } = new List<DestinationImage>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}
