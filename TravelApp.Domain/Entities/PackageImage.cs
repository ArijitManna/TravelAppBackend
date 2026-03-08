namespace TravelApp.Domain.Entities;

public class PackageImage
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    // Navigation property
    public Package Package { get; set; } = null!;
}
