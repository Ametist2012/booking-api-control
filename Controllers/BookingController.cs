using Microsoft.AspNetCore.Mvc;

namespace BookingApiControl.Controllers;

[ApiController]
[Route("api/bookings/2000")]
public class BookingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from BookingController!");
    }
}