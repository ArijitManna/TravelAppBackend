# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TravelApp.slnx ./
COPY TravelApp.API/TravelApp.API.csproj TravelApp.API/
COPY TravelApp.Application/TravelApp.Application.csproj TravelApp.Application/
COPY TravelApp.Domain/TravelApp.Domain.csproj TravelApp.Domain/
COPY TravelApp.Infrastructure/TravelApp.Infrastructure.csproj TravelApp.Infrastructure/

# Restore dependencies
RUN dotnet restore TravelApp.API/TravelApp.API.csproj

# Copy all source files
COPY . .

# Build the application
WORKDIR /src/TravelApp.API
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN apt-get update && apt-get install -y curl

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published files
COPY --from=publish /app/publish .

# Change ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "TravelApp.API.dll"]
