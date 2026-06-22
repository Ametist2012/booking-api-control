using Microsoft.AspNetCore.Mvc;
using BookingApiControl.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookingApiControl.Controllers;

[ApiController]
[Route ("api/booking")]
public class BookingController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingController (BookingService bookingService) { _bookingService = bookingService; }


    [Authorize]
    [HttpGet]
    public IActionResult GetBookingsCurUser()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) { return Unauthorized(); }
        var userId = Guid.Parse(userIdStr);

        var result = _bookingService.GetBookingsCurUserById(userId);
        return Ok(result);
    }



    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetBookingByIdCurUser(Guid id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) { return Unauthorized(); }
        var userId = Guid.Parse(userIdStr);

        var result = _bookingService.GetBookingByIdCurUser(userId, id);
        return Ok(result);
    }



    [Authorize]
    [HttpPost]
    public IActionResult BookingSetDate(CreateBookingRequest request)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) { return Unauthorized(); }
        var userId = Guid.Parse(userIdStr);

        var result = _bookingService.BookingSetDateBy(request, userId);

       if (!result.Success)
        return BadRequest(result.Error);

            return Ok("Booking created");
    }

}