using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

namespace BookingApiControl.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthService(AppDbContext db, JwtTokenService jwt) { _db = db;  _jwt = jwt; }

    
    
    public bool Register(RegisterRequest request)
    {
        var userExists = _db.Users.Any(x => x.Email == request.Email);
        if (userExists) return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User.ToString()
        };

        _db.Users.Add(user);
        _db.SaveChanges();

            return true;
    }

    

    public string? Login(LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == request.Email);
        if (user == null) return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordValid) return null;

            return _jwt.CreateToken(user);
    }
}