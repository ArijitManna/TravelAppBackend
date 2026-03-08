using Microsoft.AspNetCore.Mvc;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;

namespace TravelApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    /// <summary>
    /// Get all packages with optional filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? destination,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? category,
        [FromQuery] bool? isFeatured)
    {
        try
        {
            var filter = new PackageFilterRequest
            {
                DestinationId = destination,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Category = category,
                IsFeatured = isFeatured
            };

            var packages = await _packageService.GetAllAsync(filter);
            return Ok(packages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving packages", error = ex.Message });
        }
    }

    /// <summary>
    /// Get package by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var package = await _packageService.GetByIdAsync(id);
            if (package == null)
            {
                return NotFound(new { message = $"Package with ID {id} not found" });
            }
            return Ok(package);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the package", error = ex.Message });
        }
    }
}
