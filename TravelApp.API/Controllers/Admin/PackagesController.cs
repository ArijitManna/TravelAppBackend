using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;

namespace TravelApp.API.Controllers.Admin;

[ApiController]
[Route("api/admin/packages")]
[Authorize(Roles = "Admin")]
public class AdminPackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public AdminPackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    /// <summary>
    /// Create a new package
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageRequest request)
    {
        try
        {
            var package = await _packageService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { id = package.Id }, package);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to create package", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing package
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePackageRequest request)
    {
        try
        {
            var package = await _packageService.UpdateAsync(id, request);
            return Ok(package);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to update package", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a package
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _packageService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Package with ID {id} not found" });
            }
            return Ok(new { message = "Package deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to delete package", error = ex.Message });
        }
    }
}
