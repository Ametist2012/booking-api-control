using Microsoft.AspNetCore.Mvc;
using BookingApiControl.Data;

using BookingApiControl.Models.Enums;

using Microsoft.AspNetCore.Authorization;

namespace BookingApiControl.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminController(AppDbContext db) { _db = db; }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public IActionResult Get()
    {
    var users = _db.Users.ToList();
    return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users")]
    public IActionResult Post(AdminRegisterRequest request) 
    {
        var usExists = _db.Users.Any(w => w.Email == request.Email);  //Есть ли подходящий email в БД?
        if (usExists) return BadRequest("User already exists");       //Пользователь существует
        if (!Enum.IsDefined(typeof(Role), request.Role)) { return BadRequest("Invalid role"); }
        var newUser = new User                                        
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };
        _db.Users.Add(newUser);
        _db.SaveChanges();
        return Ok("User register success");   //User создан успешно
    }

}