using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;
    public BookingRepository(AppDbContext db) { _db = db; }


    public List<Booking> GetByUserId(Guid userId)
    {
        return _db.Bookings.Where(w => w.UserId == userId).ToList();
    }

    public List<Booking> GetList()
    {
        return _db.Bookings.ToList();
    }

    public Booking? GetByIdAndUserId(Guid bookingId, Guid userId)
    {
        return _db.Bookings.FirstOrDefault(x => x.UserId == userId && x.Id == bookingId);
    }

    public Booking? GetById(Guid bookingId)
    {
        return _db.Bookings.FirstOrDefault(x => x.Id == bookingId);
    }

    public bool IsDuplicateBooking(Guid userId, Guid roomId, DateTime checkIn, DateTime checkOut)
    {
        return _db.Bookings.Any(b => 
                b.UserId == userId &&
                b.RoomId == roomId &&
                b.CheckIn == checkIn &&
                b.CheckOut == checkOut &&
                b.Status != BookingStatus.Cancelled);
    }

    public bool IsRoomBusy(Guid roomId, DateTime checkIn, DateTime checkOut)
    {
        return _db.Bookings.Any(b =>
                b.RoomId == roomId &&
                b.Status == BookingStatus.Approved &&
                checkIn < b.CheckOut &&
                checkOut > b.CheckIn);
    }

    public void Add(Booking booking)
    {
        _db.Bookings.Add(booking);
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
}