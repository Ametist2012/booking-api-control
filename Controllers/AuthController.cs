using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingApiControl.Services;

namespace BookingApiControl.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
private readonly AuthService _authService;
public AuthController(AuthService authService)
{
    _authService = authService;
}


[AllowAnonymous] //Доступны без авторизации
[HttpPost("register")]
public IActionResult Register(RegisterRequest request)
{
    var success = _authService.Register(request);

    if (!success) return BadRequest("User already exists");

    return Ok("User register success");
}


[AllowAnonymous] //Доступны без авторизации
[HttpPost("login")]
public IActionResult Login(LoginRequest request)
{
    var token = _authService.Login(request);
    if (token == null) return Unauthorized("Invalid email or password");
    
    return Ok(new { token });
}

}


