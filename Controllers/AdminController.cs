using Microsoft.AspNetCore.Mvc;
using BookingApiControl.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookingApiControl.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService) { _adminService = adminService; }



    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public IActionResult ListUsers()
    {
        var users = _adminService.GetUsers();
            return Ok(users);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("bookings")]
    public IActionResult ListBookings()
    {
        var bookings = _adminService.GetListBookings();
        return Ok(bookings);
    }
    

    //Подтверждение бронирования
    [Authorize(Roles = "Admin")]
    [HttpPut("bookings/{BookingId}/approve")]
    public IActionResult BookingsApprove(Guid BookingId)
    {
        var bookingsApprove = _adminService.BookingsApproveUser(BookingId);
        if (!bookingsApprove.Success) return BadRequest(bookingsApprove.Error);
            return Ok(bookingsApprove);
    }


    //Отмена бронирования
    [Authorize(Roles = "Admin")]
    [HttpPut("bookings/{BookingId}/reject")]
    public IActionResult BookingsReject(Guid BookingId)
    {
        var bookingsRejectUser = _adminService.BookingsRejectUser(BookingId);
        if (!bookingsRejectUser.Success) return BadRequest(bookingsRejectUser.Error);
        return Ok(bookingsRejectUser);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("users/{userId}")]
    public IActionResult DeleteUser(Guid userId)
    {
        var result = _adminService.DelUserById(userId);
        if (!result) return NotFound("User not Fount Id");
            return Ok("User delete Success");
    }
}