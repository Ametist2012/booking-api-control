using BookingApiControl.Data;
using BookingApiControl.Models;
using BookingApiControl.Models.Enums;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _db;

    public RoomRepository(AppDbContext db)
    {
        _db = db;
    }

    // получить комнату по id
    public Room? GetById(Guid id)
    {
        return _db.Rooms.FirstOrDefault(r => r.Id == id);
    }

    // получить все комнаты
    public List<Room> GetAll()
    {
        return _db.Rooms.ToList();
    }

    // проверить существует ли комната
    public bool IdExists(Guid id)
    {
        return _db.Rooms.Any(r => r.Id == id);
    }

    public bool NumberExits(string Number)
    {
        return _db.Rooms.Any(x => x.Number == Number); 
    }

    // добавить комнату
    public void Add(Room room)
    {
        _db.Rooms.Add(room);
    }

    // удалить комнату
    public void Delete(Room room)
    {
        _db.Rooms.Remove(room);
    }

    // сохранить изменения
    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    // комнаты, доступные по датам
    public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
    {
        var busyRoomIds = _db.Bookings
            .Where(b =>
                b.Status == BookingStatus.Approved &&
                checkIn < b.CheckOut &&
                checkOut > b.CheckIn)
            .Select(b => b.RoomId)
            .ToList();

        return _db.Rooms
            .Where(r => !busyRoomIds.Contains(r.Id))
            .ToList();
    }
}