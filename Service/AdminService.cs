using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

namespace BookingApiControl.Services;

public class AdminService
{
    public readonly IBookingRepository _bookingRepository;
    //private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    public AdminService(
        IBookingRepository bookingRepository,
        //IRoomRepository roomRepository,
        IUserRepository userRepository) 
    {   
        _bookingRepository = bookingRepository; 
        //_roomRepository = roomRepository;
        _userRepository = userRepository;
    }

    public List<User> GetUsers()
    {
        return _userRepository.GetAll();
    }



    public List<Booking> GetListBookings()
    {
        return _bookingRepository.GetList();  
    }


    public ResultBoolString BookingsApproveUser(Guid BookingId)
    {
        var booking = _bookingRepository.GetById(BookingId);
        if (booking == null) return new ResultBoolString{Success = false, Error = "Booking Id not found"};

        if (booking.Status != BookingStatus.Pending)
            return new ResultBoolString { Success = false, Error = "Only pending bookings can be approved" };

        bool isBusy = _bookingRepository.IsRoomBusy(booking.RoomId, booking.CheckIn, booking.CheckOut);
        if (isBusy) return new ResultBoolString{Success = false, Error = "Room is already booked for selected dates"};
            
        booking.Status = BookingStatus.Approved;

        _bookingRepository.SaveChanges();
            return new ResultBoolString{Success = true, Error = null};
    }



    public ResultBoolString BookingsRejectUser(Guid BookingId)
    {
        var booking = _bookingRepository.GetById(BookingId);
        if (booking == null) return new ResultBoolString{Success = false, Error = "Booking Id not found"};

        if (booking.Status != BookingStatus.Pending)
            return new ResultBoolString { Success = false, Error = "Only pending bookings can be rejected" };

        booking.Status = BookingStatus.Rejected;

        _bookingRepository.SaveChanges();
            return new ResultBoolString{Success = true, Error = null};
    }


    public bool DelUserById(Guid id)
    {
        var curUser = _userRepository.GetById(id);
        if (curUser == null) return false;

        _userRepository.Delete(curUser);
        _userRepository.SaveChanges();
            return true;
    }
}