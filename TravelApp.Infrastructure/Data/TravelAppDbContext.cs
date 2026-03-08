using Microsoft.EntityFrameworkCore;
using TravelApp.Domain.Entities;

namespace TravelApp.Infrastructure.Data;

public class TravelAppDbContext : DbContext
{
    public TravelAppDbContext(DbContextOptions<TravelAppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<DestinationImage> DestinationImages { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageImage> PackageImages { get; set; }
    public DbSet<ItineraryDay> ItineraryDays { get; set; }
    public DbSet<ItineraryDayImage> ItineraryDayImages { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityImage> ActivityImages { get; set; }
    public DbSet<PackageInclusion> PackageInclusions { get; set; }
    public DbSet<PackageHighlight> PackageHighlights { get; set; }
    public DbSet<ItineraryDayHotel> ItineraryDayHotels { get; set; }
    public DbSet<ItineraryDayActivity> ItineraryDayActivities { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingTraveler> BookingTravelers { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Role Configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();

            // Seed default roles
            entity.HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Customer" }
            );
        });

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(150)
                .IsRequired();
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();
            entity.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();
            entity.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);
            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId);
        });

        // Destination Configuration
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.ToTable("destinations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(150)
                .IsRequired();
            entity.Property(e => e.Country)
                .HasColumnName("country")
                .HasMaxLength(150);
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // DestinationImage Configuration
        modelBuilder.Entity<DestinationImage>(entity =>
        {
            entity.ToTable("destination_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500)
                .IsRequired();

            entity.HasOne(e => e.Destination)
                .WithMany(d => d.Images)
                .HasForeignKey(e => e.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Package Configuration
        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("packages");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.DurationDays)
                .HasColumnName("duration_days")
                .IsRequired();
            entity.Property(e => e.BasePrice)
                .HasColumnName("base_price")
                .HasColumnType("numeric(12,2)")
                .IsRequired();
            entity.Property(e => e.OriginalPrice)
                .HasColumnName("original_price")
                .HasColumnType("numeric(12,2)");
            entity.Property(e => e.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("numeric(12,2)");
            entity.Property(e => e.MaxPeople)
                .HasColumnName("max_people");
            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasMaxLength(100);
            entity.Property(e => e.ThemeTags)
                .HasColumnName("theme_tags")
                .HasMaxLength(500);
            entity.Property(e => e.IsFeatured)
                .HasColumnName("is_featured")
                .HasDefaultValue(false);
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            entity.Property(e => e.StartDate)
                .HasColumnName("start_date");
            entity.Property(e => e.EndDate)
                .HasColumnName("end_date");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Destination)
                .WithMany(d => d.Packages)
                .HasForeignKey(e => e.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.DestinationId)
                .HasDatabaseName("idx_packages_destination");
        });

        // PackageImage Configuration
        modelBuilder.Entity<PackageImage>(entity =>
        {
            entity.ToTable("package_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired();

            entity.HasOne(e => e.Package)
                .WithMany(p => p.PackageImages)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItineraryDay Configuration
        modelBuilder.Entity<ItineraryDay>(entity =>
        {
            entity.ToTable("itinerary_days");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.DayNumber)
                .HasColumnName("day_number")
                .IsRequired();
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(200);
            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.HasOne(e => e.Package)
                .WithMany(p => p.ItineraryDays)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItineraryDayImage Configuration
        modelBuilder.Entity<ItineraryDayImage>(entity =>
        {
            entity.ToTable("itinerary_day_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItineraryDayId).HasColumnName("itinerary_day_id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500)
                .IsRequired();

            entity.HasOne(e => e.ItineraryDay)
                .WithMany(i => i.Images)
                .HasForeignKey(e => e.ItineraryDayId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItineraryDay meal inclusions update
        modelBuilder.Entity<ItineraryDay>(entity =>
        {
            entity.Property(e => e.IsBreakfastIncluded)
                .HasColumnName("is_breakfast_included")
                .HasDefaultValue(false);
            entity.Property(e => e.IsLunchIncluded)
                .HasColumnName("is_lunch_included")
                .HasDefaultValue(false);
            entity.Property(e => e.IsDinnerIncluded)
                .HasColumnName("is_dinner_included")
                .HasDefaultValue(false);
            entity.Property(e => e.IsLeisureDay)
                .HasColumnName("is_leisure_day")
                .HasDefaultValue(false);
        });

        // Hotel Configuration
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("hotels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.Address)
                .HasColumnName("address")
                .HasMaxLength(500);
            entity.Property(e => e.Rating)
                .HasColumnName("rating")
                .HasColumnType("numeric(3,2)");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");
            entity.Property(e => e.CheckInTime)
                .HasColumnName("check_in_time")
                .HasMaxLength(10);
            entity.Property(e => e.CheckOutTime)
                .HasColumnName("check_out_time")
                .HasMaxLength(10);
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // HotelImage Configuration
        modelBuilder.Entity<HotelImage>(entity =>
        {
            entity.ToTable("hotel_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500)
                .IsRequired();

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.Images)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Activity Configuration
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("activities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url");
            entity.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("numeric(12,2)");
            entity.Property(e => e.Duration)
                .HasColumnName("duration")
                .HasMaxLength(50);
            entity.Property(e => e.Location)
                .HasColumnName("location")
                .HasMaxLength(200);
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // ActivityImage Configuration
        modelBuilder.Entity<ActivityImage>(entity =>
        {
            entity.ToTable("activity_images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(500)
                .IsRequired();

            entity.HasOne(e => e.Activity)
                .WithMany(a => a.Images)
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PackageInclusion Configuration
        modelBuilder.Entity<PackageInclusion>(entity =>
        {
            entity.ToTable("package_inclusions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.InclusionType)
                .HasColumnName("inclusion_type")
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.IconName)
                .HasColumnName("icon_name")
                .HasMaxLength(50);
            entity.Property(e => e.IsIncluded)
                .HasColumnName("is_included")
                .HasDefaultValue(true);

            entity.HasOne(e => e.Package)
                .WithMany(p => p.Inclusions)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PackageHighlight Configuration
        modelBuilder.Entity<PackageHighlight>(entity =>
        {
            entity.ToTable("package_highlights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Highlight)
                .HasColumnName("highlight")
                .HasMaxLength(1000)
                .IsRequired();
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order");

            entity.HasOne(e => e.Package)
                .WithMany(p => p.Highlights)
                .HasForeignKey(e => e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItineraryDayHotel Configuration
        modelBuilder.Entity<ItineraryDayHotel>(entity =>
        {
            entity.ToTable("itinerary_day_hotels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItineraryDayId).HasColumnName("itinerary_day_id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order");
            entity.Property(e => e.IsRecommended)
                .HasColumnName("is_recommended")
                .HasDefaultValue(false);

            entity.HasOne(e => e.ItineraryDay)
                .WithMany(i => i.Hotels)
                .HasForeignKey(e => e.ItineraryDayId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.ItineraryDayHotels)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItineraryDayActivity Configuration
        modelBuilder.Entity<ItineraryDayActivity>(entity =>
        {
            entity.ToTable("itinerary_day_activities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItineraryDayId).HasColumnName("itinerary_day_id");
            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.IsRecommended)
                .HasColumnName("is_recommended")
                .HasDefaultValue(true);
            entity.Property(e => e.IsIncluded)
                .HasColumnName("is_included")
                .HasDefaultValue(false);
            entity.Property(e => e.DisplayOrder)
                .HasColumnName("display_order");

            entity.HasOne(e => e.ItineraryDay)
                .WithMany(i => i.Activities)
                .HasForeignKey(e => e.ItineraryDayId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Activity)
                .WithMany(a => a.ItineraryDayActivities)
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Transfer Configuration
        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.ToTable("transfers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItineraryDayId).HasColumnName("itinerary_day_id");
            entity.Property(e => e.VehicleType)
                .HasColumnName("vehicle_type")
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(e => e.PickupLocation)
                .HasColumnName("pickup_location")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.DropLocation)
                .HasColumnName("drop_location")
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(e => e.PickupTime)
                .HasColumnName("pickup_time")
                .HasMaxLength(10);
            entity.Property(e => e.IsPrivate)
                .HasColumnName("is_private")
                .HasDefaultValue(true);

            entity.HasOne(e => e.ItineraryDay)
                .WithMany(i => i.Transfers)
                .HasForeignKey(e => e.ItineraryDayId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Booking Configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TravelDate)
                .HasColumnName("travel_date")
                .IsRequired();
            entity.Property(e => e.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("numeric(12,2)")
                .IsRequired();
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Package)
                .WithMany(p => p.Bookings)
                .HasForeignKey(e => e.PackageId);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(e => e.UserId);

            entity.HasIndex(e => e.UserId)
                .HasDatabaseName("idx_bookings_user");
            entity.HasIndex(e => e.Status)
                .HasDatabaseName("idx_bookings_status");
        });

        // BookingTraveler Configuration
        modelBuilder.Entity<BookingTraveler>(entity =>
        {
            entity.ToTable("booking_travelers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(150);
            entity.Property(e => e.Age)
                .HasColumnName("age");
            entity.Property(e => e.Gender)
                .HasColumnName("gender")
                .HasMaxLength(20);

            entity.HasOne(e => e.Booking)
                .WithMany(b => b.BookingTravelers)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment Configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.PaymentGateway)
                .HasColumnName("payment_gateway")
                .HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasColumnName("transaction_id")
                .HasMaxLength(200);
            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .HasColumnType("numeric(12,2)");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50);
            entity.Property(e => e.PaidAt)
                .HasColumnName("paid_at");

            entity.HasOne(e => e.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(e => e.BookingId);
        });
    }
}
