using BookingApiControl.Data;
using BookingApiControl.Models.Enums;


namespace BookingApiControl.Services;


public class BookingService
{
    public readonly AppDbContext _db;

    public BookingService(AppDbContext db) { _db = db;}


    public List<Booking> GetBookingsCurUserById(Guid userid)
    {
       return _db.Bookings.Where(w => w.UserId == userid).ToList();
    }



    public List<Booking> GetBookingByIdCurUser(Guid userid, Guid id)
    {
        return _db.Bookings.Where(x => x.UserId == userid && x.Id == id).ToList();
    }



    public ResultBoolString BookingSetDateBy(CreateBookingRequest request, Guid userId)
    {
        // Проверка пользователя
        var userExists = _db.Users.Any(u => u.Id == userId);
        if (!userExists) 
            return new ResultBoolString{Success = false, Error = "User not found"};

        // Проверка комнаты
        var room = _db.Rooms.FirstOrDefault(r => r.Id == request.roomId);
        if (room == null) 
            return new ResultBoolString{Success = false, Error = "Room not found"};

        // Проверка дат(одинаковые даты/неправильный диапазон)
        if (request.CheckIn >= request.CheckOut) 
            return new ResultBoolString{Success = false, Error = "Check-out must be later than check-in"};

        // Дата в прошлом
        if (request.CheckIn.Date < DateTime.UtcNow.Date) 
            return new ResultBoolString{Success = false, Error = "Check-in cannot be in the past"};

        // Максимум 90 дней для брони
        if ((request.CheckOut - request.CheckIn).TotalDays > 90) 
            return new ResultBoolString{Success = false, Error = "Maximum booking period is 90 days"};

        // Пользователь уже создал такую же бронь
        var duplicateBooking = _db.Bookings.Any(b =>
            b.UserId == userId &&
            b.RoomId == request.roomId &&
            b.CheckIn == request.CheckIn &&
            b.CheckOut == request.CheckOut &&
            b.Status != BookingStatus.Cancelled);

        //Бронирование уже есть
        if (duplicateBooking) 
            return new ResultBoolString{Success = false, Error = "You already have this booking"};

        // Комната занята в выбранные даты
        var roomBusy = _db.Bookings.Any(b =>
            b.RoomId == request.roomId &&
            b.Status == BookingStatus.Approved &&
            request.CheckIn < b.CheckOut &&
            request.CheckOut > b.CheckIn);
        if (roomBusy) 
            return new ResultBoolString{Success = false, Error = "Room is already booked for these dates"};

        //NewBooking
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoomId = request.roomId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = BookingStatus.Pending
        };

        _db.Bookings.Add(booking);
        _db.SaveChanges();

    return new ResultBoolString{Success = true, Error = null};
    }   
}