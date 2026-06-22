public class CreateBookingRequest
{
    public Guid roomId {get; set;}
    public DateTime CheckIn { get; set; } //Дата начала бронирования
    public DateTime CheckOut { get; set; } //Дата окончания бронирования
}