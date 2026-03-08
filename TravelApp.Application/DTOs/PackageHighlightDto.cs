namespace TravelApp.Application.DTOs;

public class PackageHighlightDto
{
    public int Id { get; set; }
    public string Highlight { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class CreatePackageHighlightRequest
{
    public string Highlight { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
