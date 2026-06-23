using BookingApiControl.Models.Enums;


namespace BookingApiControl.Services;


public class BookingService
{
    public readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    public BookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IUserRepository userRepository) 
    {   
        _bookingRepository = bookingRepository; 
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }


    public List<Booking> GetBookingsCurUserById(Guid userid)
    {
        return _bookingRepository.GetByUserId(userid); 
    }

    public Booking? GetBookingByIdCurUser(Guid id, Guid userid)
    {
        return _bookingRepository.GetByIdAndUserId(id,userid); 
    }



    public ResultBoolString BookingSetDateBy(CreateBookingRequest request, Guid userId)
    {
        // Проверка пользователя
        var userExists = _userRepository.Exists(userId);
        if (!userExists) 
            return new ResultBoolString{Success = false, Error = "User not found"};

        // Проверка комнаты
        var room = _roomRepository.GetById(request.roomId);
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
        var duplicateBooking = _bookingRepository.IsDuplicateBooking(userId, request.roomId, request.CheckIn, request.CheckOut);
        if (duplicateBooking)  //Бронирование уже есть
            return new ResultBoolString{Success = false, Error = "You already have this booking"};

        // Комната занята в выбранные даты
        var roomBusy = _bookingRepository.IsRoomBusy(request.roomId, request.CheckIn, request.CheckOut);
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

        _bookingRepository.Add(booking);
        _bookingRepository.SaveChanges();

            return new ResultBoolString{Success = true};
    }   
}