using BookingApiControl.Data;
using BookingApiControl.Models.Enums;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    // получить пользователя по id
    public User? GetById(Guid id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    // получить по email
    public User? GetByEmail(string email)
    {
        return _db.Users.FirstOrDefault(u => u.Email == email);
    }

    // проверить существует ли пользователь
    public bool Exists(Guid id)
    {
        return _db.Users.Any(u => u.Id == id);
    }

    // проверка email
    public bool EmailExists(string email)
    {
        return _db.Users.Any(e => e.Email == email);
    }

    // получить всех пользователей
    public List<User> GetAll()
    {
        return _db.Users.ToList();
    }

    // только админы
    public List<User> GetAdmins()
    {
        return _db.Users
            .Where(u => u.Role == Role.Admin.ToString())
            .ToList();
    }

    // добавить пользователя
    public void Add(User user)
    {
        _db.Users.Add(user);
    }

    public void Delete(User user)
    {
        _db.Users.Remove(user);
    }

    // сохранить изменения
    public void SaveChanges()
    {
        _db.SaveChanges();
    }
}