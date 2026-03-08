namespace TravelApp.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string? PaymentGateway { get; set; }
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public DateTime? PaidAt { get; set; }
    
    // Navigation property
    public Booking Booking { get; set; } = null!;
}
