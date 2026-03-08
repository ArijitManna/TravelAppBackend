namespace TravelApp.Domain.Entities;

public class PackageInclusion
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string InclusionType { get; set; } = string.Empty; // Transfer, Stay, Breakfast, Lunch, Dinner, Sightseeing, etc.
    public string? IconName { get; set; }
    public bool IsIncluded { get; set; } = true;
    
    // Navigation property
    public Package Package { get; set; } = null!;
}
