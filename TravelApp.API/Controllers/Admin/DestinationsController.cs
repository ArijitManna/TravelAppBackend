using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;

namespace TravelApp.API.Controllers.Admin;

[ApiController]
[Route("api/admin/destinations")]
[Authorize(Roles = "Admin")]
public class AdminDestinationsController : ControllerBase
{
    private readonly IDestinationService _destinationService;

    public AdminDestinationsController(IDestinationService destinationService)
    {
        _destinationService = destinationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDestinationRequest request)
    {
        try
        {
            var destination = await _destinationService.CreateAsync(request);
            return CreatedAtAction(nameof(Create), new { id = destination.Id }, destination);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to create destination", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDestinationRequest request)
    {
        try
        {
            var destination = await _destinationService.UpdateAsync(id, request);
            return Ok(destination);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to update destination", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _destinationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Destination with ID {id} not found" });
            }
            return Ok(new { message = "Destination deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to delete destination", error = ex.Message });
        }
    }
}
