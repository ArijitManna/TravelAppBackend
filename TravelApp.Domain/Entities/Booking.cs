namespace TravelApp.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public int PackageId { get; set; }
    public Guid UserId { get; set; }
    public DateTime TravelDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Package Package { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<BookingTraveler> BookingTravelers { get; set; } = new List<BookingTraveler>();
    public Payment? Payment { get; set; }
}
