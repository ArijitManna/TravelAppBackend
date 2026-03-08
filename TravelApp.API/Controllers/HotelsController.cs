using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Application.DTOs;
using TravelApp.Application.Interfaces;

namespace TravelApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<HotelDto>>> GetAll()
    {
        var hotels = await _hotelService.GetAllAsync();
        return Ok(hotels);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetById(int id)
    {
        var hotel = await _hotelService.GetByIdAsync(id);
        if (hotel == null)
        {
            return NotFound(new { message = $"Hotel with ID {id} not found" });
        }
        return Ok(hotel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<HotelDto>> Create([FromBody] CreateHotelRequest request)
    {
        try
        {
            var hotel = await _hotelService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<HotelDto>> Update(int id, [FromBody] UpdateHotelRequest request)
    {
        try
        {
            var hotel = await _hotelService.UpdateAsync(id, request);
            return Ok(hotel);
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
        var result = await _hotelService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Hotel with ID {id} not found" });
        }
        return NoContent();
    }
}
