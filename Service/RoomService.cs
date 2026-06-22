using BookingApiControl.Data;
using BookingApiControl.Models;

namespace BookingApiControl.Services;

public class RoomService
{
    private readonly AppDbContext _db;

    public RoomService(AppDbContext db) { _db = db; }



    public List<Room> GetRoomList()
    {
        return _db.Rooms.ToList();
    }



    public Room? GetRoomById(Guid id)
    {
        return _db.Rooms.FirstOrDefault(r => r.Id == id);
    }



    public bool CreateRoom(CreateRoomRequest request)
    {
        var roomExists = _db.Rooms.Any(x => x.Number == request.Number);
        if (roomExists) return false;

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Number = request.Number,
            Capacity = request.Capacity,
            Class = request.Class,
            Price = request.Price,
            Description = request.Description
        };

        _db.Rooms.Add(room);
        _db.SaveChanges();
            return true;
    }



    public bool EditingRoomById(Guid id, EditingRoomRequest request)
    {
        var room = _db.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return false;

        if (request.Number != null)
            room.Number = request.Number;

        if (request.Capacity != null)
            room.Capacity = request.Capacity.Value;

        if (request.Class != null)
            room.Class = request.Class.Value;

        if (request.Price != null)
            room.Price = request.Price.Value;

        if (!string.IsNullOrWhiteSpace(request.Description))
            room.Description = request.Description;

        _db.SaveChanges();
            return true;
    }



    public bool DeleteRoomById(Guid id)
    {
        var room = _db.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return false;
        
        _db.Rooms.Remove(room);
        _db.SaveChanges();

            return true;

    }

}