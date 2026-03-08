namespace TravelApp.Application.DTOs;

public class PackageInclusionDto
{
    public int Id { get; set; }
    public string InclusionType { get; set; } = string.Empty;
    public string? IconName { get; set; }
    public bool IsIncluded { get; set; }
}

public class CreatePackageInclusionRequest
{
    public string InclusionType { get; set; } = string.Empty;
    public string? IconName { get; set; }
    public bool IsIncluded { get; set; } = true;
}
