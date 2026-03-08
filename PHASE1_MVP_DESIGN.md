# 🚀 PHASE 1 – DATABASE + API DESIGN (PostgreSQL)

## Tech Stack
- ✅ PostgreSQL
- ✅ .NET 8 Web API
- ✅ JWT Auth
- ✅ Clean but scalable structure

Production-ready but MVP simple.

---

## Module Design Overview

1. Auth & Users
2. Destinations
3. Packages
4. Booking
5. Payments
6. Admin Dashboard

---

## 🗄️ 1️⃣ AUTH & USERS MODULE

### 📌 Tables

#### `roles`
```sql
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);
```

**Default rows:**
- Admin
- Customer

#### `users`
```sql
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    full_name VARCHAR(150) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    phone VARCHAR(20),
    role_id INT REFERENCES roles(id),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### 🔌 APIs

#### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

**Response:**
```json
{
  "token": "jwt_token",
  "role": "Customer",
  "userId": "uuid"
}
```

---

## 🗺️ 2️⃣ DESTINATION MODULE

### 📌 Table: `destinations`
```sql
CREATE TABLE destinations (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    country VARCHAR(150),
    description TEXT,
    image_url TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### 🔌 APIs

#### Public
- `GET /api/destinations`
- `GET /api/destinations/{id}`

#### Admin
- `POST /api/admin/destinations`
- `PUT /api/admin/destinations/{id}`
- `DELETE /api/admin/destinations/{id}`

---

## 🧳 3️⃣ PACKAGE MODULE

### 📌 Tables

#### `packages`
```sql
CREATE TABLE packages (
    id SERIAL PRIMARY KEY,
    destination_id INT REFERENCES destinations(id) ON DELETE CASCADE,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    duration_days INT NOT NULL,
    base_price NUMERIC(12,2) NOT NULL,
    max_people INT,
    category VARCHAR(100),
    is_featured BOOLEAN DEFAULT FALSE,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### `package_images`
```sql
CREATE TABLE package_images (
    id SERIAL PRIMARY KEY,
    package_id INT REFERENCES packages(id) ON DELETE CASCADE,
    image_url TEXT NOT NULL
);
```

#### `itinerary_days`
```sql
CREATE TABLE itinerary_days (
    id SERIAL PRIMARY KEY,
    package_id INT REFERENCES packages(id) ON DELETE CASCADE,
    day_number INT NOT NULL,
    title VARCHAR(200),
    description TEXT
);
```

### 🔌 APIs

#### Public
- `GET /api/packages`
- `GET /api/packages/{id}`
- `GET /api/packages?destination=1&minPrice=5000&maxPrice=20000`

#### Admin
- `POST /api/admin/packages`
- `PUT /api/admin/packages/{id}`
- `DELETE /api/admin/packages/{id}`

---

## 🧾 4️⃣ BOOKING MODULE

### 📌 Tables

#### `bookings`
```sql
CREATE TABLE bookings (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    package_id INT REFERENCES packages(id),
    user_id UUID REFERENCES users(id),
    travel_date DATE NOT NULL,
    total_amount NUMERIC(12,2) NOT NULL,
    status VARCHAR(50) DEFAULT 'Pending',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### `booking_travelers`
```sql
CREATE TABLE booking_travelers (
    id SERIAL PRIMARY KEY,
    booking_id UUID REFERENCES bookings(id) ON DELETE CASCADE,
    full_name VARCHAR(150),
    age INT,
    gender VARCHAR(20)
);
```

### 🔌 APIs

#### Customer
- `POST /api/bookings`
- `GET /api/bookings/my`

#### Admin
- `GET /api/admin/bookings`
- `PUT /api/admin/bookings/{id}/status`

**Status:**
- Pending
- Confirmed
- Cancelled

---

## 💳 5️⃣ PAYMENT MODULE

### 📌 Table: `payments`
```sql
CREATE TABLE payments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    booking_id UUID REFERENCES bookings(id),
    payment_gateway VARCHAR(50),
    transaction_id VARCHAR(200),
    amount NUMERIC(12,2),
    status VARCHAR(50),
    paid_at TIMESTAMP
);
```

### 🔌 APIs
- `POST /api/payments/create-order`
- `POST /api/payments/verify`

### Payment Flow
1. Create booking → status `Pending`
2. Create Razorpay order
3. Verify payment
4. Update:
   - `payments.status = Success`
   - `bookings.status = Confirmed`

---

## 📊 6️⃣ ADMIN DASHBOARD API

### Endpoint
`GET /api/admin/dashboard`

**Returns:**
```json
{
  "totalBookings": 120,
  "totalRevenue": 245000,
  "activePackages": 15,
  "recentBookings": []
}
```

---

## 🔐 Security Layer

- **JWT Authentication**
- **Role-based authorization:**
  ```csharp
  [Authorize(Roles = "Admin")]
  ```
- **Refresh Token** (optional in Phase 1)

---

## 🏗️ Backend Architecture

```
TravelApp.API
TravelApp.Application
TravelApp.Domain
TravelApp.Infrastructure
```

### Technologies
- **EF Core** (Npgsql provider)
- **FluentValidation**
- **AutoMapper**
- **Repository pattern** (optional)
- **Service layer** for business logic

---

## ⚡ Important PostgreSQL Notes

### Enable UUID + Crypto
```sql
CREATE EXTENSION IF NOT EXISTS "pgcrypto";
```

### Indexes (Important for Performance)
```sql
CREATE INDEX idx_packages_destination ON packages(destination_id);
CREATE INDEX idx_bookings_user ON bookings(user_id);
CREATE INDEX idx_bookings_status ON bookings(status);
```

---

## 🎯 Phase 1 Result

After implementing this:

- ✔ Admin can manage packages
- ✔ Users can browse
- ✔ Users can book
- ✔ Payment works
- ✔ Admin can confirm bookings
- ✔ Dashboard shows stats

---

## 📝 Implementation Checklist

### Database Setup
- [ ] Create PostgreSQL database
- [ ] Enable pgcrypto extension
- [ ] Create all tables in order (roles, users, destinations, packages, etc.)
- [ ] Add default roles (Admin, Customer)
- [ ] Create indexes for performance

### Backend Setup
- [ ] Initialize .NET 8 Web API project
- [ ] Setup project structure (API, Application, Domain, Infrastructure)
- [ ] Configure Npgsql EF Core provider
- [ ] Setup JWT authentication
- [ ] Implement role-based authorization

### Module Implementation
- [ ] **Auth Module**: Register, Login, JWT generation
- [ ] **Destination Module**: CRUD for destinations (Public + Admin)
- [ ] **Package Module**: CRUD for packages, images, itinerary
- [ ] **Booking Module**: Create booking, list bookings, manage travelers
- [ ] **Payment Module**: Razorpay integration, order creation, verification
- [ ] **Admin Dashboard**: Statistics and recent bookings

### Testing
- [ ] Test auth endpoints
- [ ] Test public API endpoints
- [ ] Test admin endpoints with authorization
- [ ] Test payment flow end-to-end
- [ ] Test booking status updates

### Deployment Prep
- [ ] Setup connection strings configuration
- [ ] Setup logging
- [ ] Setup error handling middleware
- [ ] Configure CORS
- [ ] Setup API documentation (Swagger)
