namespace TravelApp.Domain.Entities;

public class PackageHighlight
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string Highlight { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    
    // Navigation property
    public Package Package { get; set; } = null!;
}
