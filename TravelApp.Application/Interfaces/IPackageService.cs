using TravelApp.Application.DTOs;

namespace TravelApp.Application.Interfaces;

public interface IPackageService
{
    Task<IEnumerable<PackageDto>> GetAllAsync(PackageFilterRequest? filter = null);
    Task<PackageDto?> GetByIdAsync(int id);
    Task<PackageDto> CreateAsync(CreatePackageRequest request);
    Task<PackageDto> UpdateAsync(int id, UpdatePackageRequest request);
    Task<bool> DeleteAsync(int id);
}
