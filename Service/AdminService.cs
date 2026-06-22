using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

namespace BookingApiControl.Services;

public class AdminService
{
    private readonly AppDbContext _db;

    public AdminService(AppDbContext db) { _db = db; }

    public List<User> GetUsers()
    {
        return _db.Users.ToList();
    }



    public List<Booking> GetListBookings()
    {
        return _db.Bookings.ToList();
    }


    public ResultBoolString BookingsApproveUser(Guid BookingId)
    {
        var booking = _db.Bookings.FirstOrDefault(x => x.Id == BookingId);
        if (booking == null) return new ResultBoolString{Success = false, Error = "Booking Id not found"};

        if (booking.Status != BookingStatus.Pending)
            return new ResultBoolString { Success = false, Error = "Only pending bookings can be approved" };

        bool isBusy = _db.Bookings.Any(b =>
            b.RoomId == booking.RoomId &&
            b.Status == BookingStatus.Approved &&
            booking.CheckIn < b.CheckOut &&
            booking.CheckOut > b.CheckIn);
        if (isBusy) return new ResultBoolString{Success = false, Error = "Room is already booked for selected dates"};
            
        booking.Status = BookingStatus.Approved;

        _db.SaveChanges();
            return new ResultBoolString{Success = true, Error = null};
    }



    public ResultBoolString BookingsRejectUser(Guid BookingId)
    {
        var booking = _db.Bookings.FirstOrDefault(x => x.Id == BookingId);
        if (booking == null) return new ResultBoolString{Success = false, Error = "Booking Id not found"};

        if (booking.Status != BookingStatus.Pending)
            return new ResultBoolString { Success = false, Error = "Only pending bookings can be rejected" };

        booking.Status = BookingStatus.Rejected;

        _db.SaveChanges();
            return new ResultBoolString{Success = true, Error = null};
    }


    public bool DelUserById(Guid id)
    {
        var curUser = _db.Users.FirstOrDefault(y => y.Id == id);
        if (curUser == null) return false;

        _db.Users.Remove(curUser);
        _db.SaveChanges();
            return true;
    }
}