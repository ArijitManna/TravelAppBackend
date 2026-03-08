namespace TravelApp.Domain.Entities;

public class BookingTraveler
{
    public int Id { get; set; }
    public Guid BookingId { get; set; }
    public string? FullName { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    
    // Navigation property
    public Booking Booking { get; set; } = null!;
}
