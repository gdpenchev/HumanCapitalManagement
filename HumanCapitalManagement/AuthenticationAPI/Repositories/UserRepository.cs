using AuthenticationAPI.Models;

namespace AuthenticationAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Username = "Pesho",  Password = "12345", Role = "manager"},
            new User { Username = "Ivan", Password = "12345", Role = "employee"},
            new User { Username = "Maria", Password = "12345", Role = "hradmin"}
        };

        public User ValidateUser(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
