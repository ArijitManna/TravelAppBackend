namespace TravelApp.Application.DTOs;

public class TransferDto
{
    public int Id { get; set; }
    public string VehicleType { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string DropLocation { get; set; } = string.Empty;
    public string? PickupTime { get; set; }
    public bool IsPrivate { get; set; }
}

public class CreateTransferRequest
{
    public string VehicleType { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string DropLocation { get; set; } = string.Empty;
    public string? PickupTime { get; set; }
    public bool IsPrivate { get; set; } = true;
}
