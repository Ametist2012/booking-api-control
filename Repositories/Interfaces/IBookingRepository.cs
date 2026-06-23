public interface IBookingRepository
{
   
    List<Booking> GetByUserId(Guid userId);                                              // Получить все брони пользователя
    List<Booking> GetList();                                                             // Получить все существующие брони 
    Booking? GetByIdAndUserId(Guid bookingId, Guid userId);                              // Получить одну бронь пользователя по bookingId
    Booking? GetById (Guid bookingId);                                                  // Получить одну бронь по bookingId
    bool IsDuplicateBooking(Guid userId, Guid roomId, DateTime checkIn, DateTime checkOut);// Проверка дубля брони
    bool IsRoomBusy(Guid roomId, DateTime checkIn, DateTime checkOut);                   // Проверка занятости комнаты
    void Add(Booking booking);                                                           // Добавить бронь
    void SaveChanges();                                                                  // Сохранить изменения
}