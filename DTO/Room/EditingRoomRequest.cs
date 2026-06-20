public class EditingRoomRequest
{
    public string Number { get; set; } = string.Empty;
    public int? Capacity { get; set; }   //Вместимость комнаты
    public int? Class { get; set; }      //Класс комнаты (1..5)
    public decimal? Price { get; set; }
    public string Description { get; set; } = string.Empty; //Описание команты
}