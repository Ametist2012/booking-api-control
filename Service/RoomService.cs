using BookingApiControl.Data;
using BookingApiControl.Models;

namespace BookingApiControl.Services;

public class RoomService
{
    public readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;
    public RoomService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IUserRepository userRepository) 
    {   
        _bookingRepository = bookingRepository; 
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }



    public List<Room> GetRoomList()
    {
        return _roomRepository.GetAll(); 
    }



    public Room? GetRoomById(Guid id)
    {
        return _roomRepository.GetById(id);
    }



    public bool CreateRoom(CreateRoomRequest request)
    {
        var roomExists = _roomRepository.NumberExits(request.Number); 
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

        _roomRepository.Add(room);
        _roomRepository.SaveChanges();
            return true;
    }



    public bool EditingRoomById(Guid id, EditingRoomRequest request)
    {   
        var room = _roomRepository.GetById(id);
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

        _roomRepository.SaveChanges();
            return true;
    }



    public bool DeleteRoomById(Guid id)
    {
        var room = _roomRepository.GetById(id);
        if (room == null) return false;
        
        _roomRepository.Delete(room);
        _roomRepository.SaveChanges();
            return true;

    }

}