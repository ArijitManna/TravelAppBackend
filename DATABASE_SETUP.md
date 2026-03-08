# Database Setup Script

## Prerequisites
- PostgreSQL installed and running
- Default username: `postgres`
- Default password: `postgres`

## Steps to Create Database

### 1. Create PostgreSQL Database
```sql
CREATE DATABASE travelapp_db;
```

### 2. Enable UUID Extension
Connect to `travelapp_db` and run:
```sql
CREATE EXTENSION IF NOT EXISTS "pgcrypto";
```

### 3. Apply Migrations
Run from the solution root directory:
```powershell
dotnet ef database update --project TravelApp.Infrastructure --startup-project TravelApp.API
```

## Alternative: Update Connection String

If your PostgreSQL credentials are different, update the connection string in:
- `TravelApp.API/appsettings.json`
- `TravelApp.API/appsettings.Development.json`

Example:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=travelapp_db;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  }
}
```

## Verify Database Setup

After running migrations, you should have the following tables:
- roles (with 2 default rows: Admin, Customer)
- users
- destinations
- packages
- package_images
- itinerary_days
- bookings
- booking_travelers
- payments

## Useful Commands

### Create a new migration
```powershell
dotnet ef migrations add MigrationName --project TravelApp.Infrastructure --startup-project TravelApp.API
```

### Remove last migration (if not applied)
```powershell
dotnet ef migrations remove --project TravelApp.Infrastructure --startup-project TravelApp.API
```

### Generate SQL script
```powershell
dotnet ef migrations script --project TravelApp.Infrastructure --startup-project TravelApp.API --output migration.sql
```

### Revert to a specific migration
```powershell
dotnet ef database update MigrationName --project TravelApp.Infrastructure --startup-project TravelApp.API
```
