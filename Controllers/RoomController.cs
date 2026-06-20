using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingApiControl.Services;

using BookingApiControl.Models;

namespace BookingApiControl.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
private readonly RoomService _roomService;
public RoomController(RoomService roomService)
{
    _roomService = roomService;
}

[Authorize]
[HttpGet]
public IActionResult RoomList()
{
    var rooms = _roomService.GetRoomList();

    return Ok(rooms);
}

[Authorize]
[HttpGet("{id}")]
public IActionResult RoomListbyId(Guid id)
{
    var room = _roomService.GetRoomById(id);
    if (room == null)
        return NotFound("Room not found");
    return Ok(room);
}

[Authorize(Roles = "Admin")]
[HttpPost]
public IActionResult CreateRoom(CreateRoomRequest request)
{
    var success = _roomService.CreateRoom(request);

    if (!success) return BadRequest("Create Room error");

    return Ok("Create Room success");
}

[Authorize(Roles = "Admin")]
[HttpPut("{id}")]
public IActionResult EditingRoom(Guid id, EditingRoomRequest request)
{
    var result = _roomService.EditingRoomById(id, request);
    if (!result) return NotFound();

    return Ok("Room updated");
}

[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteRoom(Guid id)
{
    var result = _roomService.DeleteRoomById(id);
    if (!result) return NotFound();

    return Ok("Room delete");
}

}








