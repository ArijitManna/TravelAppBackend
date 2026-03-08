using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;

namespace TravelApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetAll()
    {
        var activities = await _activityService.GetAllAsync();
        return Ok(activities);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetById(int id)
    {
        var activity = await _activityService.GetByIdAsync(id);
        if (activity == null)
        {
            return NotFound(new { message = $"Activity with ID {id} not found" });
        }
        return Ok(activity);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ActivityDto>> Create([FromBody] CreateActivityRequest request)
    {
        try
        {
            var activity = await _activityService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ActivityDto>> Update(int id, [FromBody] UpdateActivityRequest request)
    {
        try
        {
            var activity = await _activityService.UpdateAsync(id, request);
            return Ok(activity);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _activityService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Activity with ID {id} not found" });
        }
        return NoContent();
    }
}
