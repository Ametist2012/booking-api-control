using BookingApiControl.Models.Enums;
public class Booking
{
    public Guid Id { get; set; } //уникальный ID операции (GUID)
    public Guid RoomId { get; set; } //ID комнаты в эту операцию (GUID)
    public Guid UserId { get; set; } //ID пользователя, который бронирует (GUID)
    public DateTime CheckIn { get; set; } //Дата начала бронирования
    public DateTime CheckOut { get; set; } //Дата окончания бронирования
    public BookingStatus Status { get; set; }
}


