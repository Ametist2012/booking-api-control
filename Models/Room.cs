namespace BookingApiControl.Models;

public class Room
{
    public Guid Id { get; set; } //Уникальный ID (GUID)
    public string Number { get; set; } = string.Empty; //Номер комнаты
    public int Capacity { get; set; }   //Вместимость комнаты
    public int Class { get; set; }      //Класс комнаты (1..5)
    public decimal Price { get; set; } //Цена комнаты 
    public string Description { get; set; } = string.Empty; //Описание команты
}