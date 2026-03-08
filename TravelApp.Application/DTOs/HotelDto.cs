namespace TravelApp.Application.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public List<HotelImageDto> Images { get; set; } = new();
}

public class HotelImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class CreateHotelRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

public class UpdateHotelRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> ImageUrls { get; set; } = new();
}
