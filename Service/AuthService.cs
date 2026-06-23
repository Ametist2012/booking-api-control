using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

namespace BookingApiControl.Services;

public class AuthService
{
    private readonly JwtTokenService _jwt;
    public readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    public AuthService(
        JwtTokenService jwt,
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IUserRepository userRepository) 
    {   
        _jwt = jwt; 
        _bookingRepository = bookingRepository; 
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }
    
    
    public bool Register(RegisterRequest request)
    {
        var userExists = _userRepository.GetByEmail(request.Email);
        if (userExists == null) return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Role.User.ToString()
        };

        _userRepository.Add(user);
        _userRepository.SaveChanges();

            return true;
    }

    

    public string? Login(LoginRequest request)
    {
        var user = _userRepository.GetByEmail(request.Email);
        if (user == null) return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordValid) return null;

            return _jwt.CreateToken(user);
    }
}