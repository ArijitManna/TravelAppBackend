namespace TravelApp.Application.DTOs;

public class PackageFilterRequest
{
    public int? DestinationId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Category { get; set; }
    public bool? IsFeatured { get; set; }
}
