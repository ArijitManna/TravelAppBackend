namespace TravelApp.Application.DTOs;

public class ActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Duration { get; set; }
    public string? Location { get; set; }
    public List<ActivityImageDto> Images { get; set; } = new();
}

public class ActivityImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class CreateActivityRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Duration { get; set; }
    public string? Location { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

public class UpdateActivityRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
    public string? Duration { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> ImageUrls { get; set; } = new();
}
