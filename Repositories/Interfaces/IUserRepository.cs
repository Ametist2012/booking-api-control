public interface IUserRepository
{
    User? GetById(Guid id);
    User? GetByEmail(string email);
    bool Exists(Guid id); //Существует user
    bool EmailExists(string email);
    List<User> GetAll(); //
    List<User> GetAdmins(); //
    void Add(User user);
    void Delete(User user);
    void SaveChanges();
}