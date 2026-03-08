namespace TravelApp.Application.DTOs;

public class UpdateDestinationRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }    public List<string> ImageUrls { get; set; } = new();    public bool IsActive { get; set; }
}
