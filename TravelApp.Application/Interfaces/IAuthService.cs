using TravelApp.Application.DTOs;

namespace TravelApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> RegisterAdminAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
