using BookingApiControl.Models;

public interface IRoomRepository
{
    Room? GetById(Guid id);
    List<Room> GetAll();
    List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut);
    bool IdExists(Guid id);
    bool NumberExits(string Number);
    void Add(Room room);
    void Delete(Room room);
    void SaveChanges();
}