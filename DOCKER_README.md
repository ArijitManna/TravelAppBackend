# Docker Setup Guide

This guide explains how to run the Travel App API and PostgreSQL database using Docker.

## Prerequisites

- Docker Desktop for Windows (or Docker Engine + Docker Compose on Linux)
- At least 2GB of free disk space
- Ports 5000 and 5432 available on your host machine

## Quick Start

### 1. Environment Setup

Copy the example environment file and customize if needed:

```powershell
Copy-Item .env.example .env
```

Edit `.env` to set your own values for passwords and JWT secrets.

### 2. Build and Run

Start both the database and API:

```powershell
docker-compose up -d
```

This will:
- Pull the PostgreSQL 17 Alpine image
- Build the .NET API image
- Start both containers
- Initialize the database with `create_database.sql`
- Set up networking between containers

### 3. Verify Deployment

Check if containers are running:

```powershell
docker-compose ps
```

View logs:

```powershell
# All services
docker-compose logs -f

# Just the API
docker-compose logs -f api

# Just the database
docker-compose logs -f postgres
```

### 4. Access the Application

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **PostgreSQL**: localhost:5432
  - Database: `travelapp_db`
  - Username: `postgres`
  - Password: `Admin` (or your .env value)

## Development Mode

For development with hot reload and debugging:

```powershell
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
```

## Common Commands

### Stop Services

```powershell
docker-compose down
```

### Stop and Remove Volumes (Clean Database)

```powershell
docker-compose down -v
```

### Rebuild API Image

After code changes:

```powershell
docker-compose up -d --build api
```

### Execute Commands in Containers

Access PostgreSQL shell:

```powershell
docker exec -it travelapp-postgres psql -U postgres -d travelapp_db
```

Access API container shell:

```powershell
docker exec -it travelapp-api sh
```

### View Resource Usage

```powershell
docker stats
```

## Database Management

### Connect to PostgreSQL

Using Docker:

```powershell
docker exec -it travelapp-postgres psql -U postgres -d travelapp_db
```

Using external tool (pgAdmin, DBeaver, etc.):
- Host: localhost
- Port: 5432
- Database: travelapp_db
- Username: postgres
- Password: Admin

### Backup Database

```powershell
docker exec travelapp-postgres pg_dump -U postgres travelapp_db > backup.sql
```

### Restore Database

```powershell
Get-Content backup.sql | docker exec -i travelapp-postgres psql -U postgres -d travelapp_db
```

## Troubleshooting

### Port Already in Use

If ports 5000 or 5432 are already in use, modify `docker-compose.yml`:

```yaml
ports:
  - "5001:8080"  # Change 5000 to 5001 for API
  - "5433:5432"  # Change 5432 to 5433 for PostgreSQL
```

### Container Health Check Failures

Check container logs:

```powershell
docker-compose logs api
docker-compose logs postgres
```

Restart services:

```powershell
docker-compose restart
```

### Database Connection Issues

Ensure PostgreSQL is healthy before API starts:

```powershell
docker-compose up -d postgres
# Wait 10 seconds
docker-compose up -d api
```

### Clean Start

Remove everything and start fresh:

```powershell
docker-compose down -v
docker system prune -f
docker-compose up -d --build
```

## Production Deployment

For production environments:

1. **Use strong passwords**: Update `.env` with secure values
2. **Use secrets management**: Consider Docker secrets or cloud provider secret services
3. **Enable HTTPS**: Configure reverse proxy (nginx, Traefik) with SSL certificates
4. **Resource limits**: Add resource constraints in `docker-compose.yml`
5. **Monitoring**: Implement logging and monitoring solutions
6. **Backups**: Set up automated database backups

Example with resource limits:

```yaml
api:
  deploy:
    resources:
      limits:
        cpus: '1'
        memory: 512M
      reservations:
        cpus: '0.5'
        memory: 256M
```

## Network Architecture

```
┌─────────────────┐
│   Host Machine  │
│  localhost:5000 │
└────────┬────────┘
         │
         │ Port 5000:8080
         ↓
┌─────────────────────────┐
│   travelapp-network     │
│  (Docker Bridge)        │
│                         │
│  ┌──────────────┐      │
│  │  API         │      │
│  │  :8080       │──────┼──→ postgres:5432
│  └──────────────┘      │
│                         │
│  ┌──────────────┐      │
│  │  PostgreSQL  │      │
│  │  :5432       │      │
│  └──────────────┘      │
└─────────────────────────┘
         │
         │ Port 5432:5432
         ↓
┌─────────────────┐
│   Host Machine  │
│  localhost:5432 │
└─────────────────┘
```

## Support

For issues or questions, check:
- Docker logs: `docker-compose logs -f`
- Container status: `docker-compose ps`
- System resources: `docker stats`
