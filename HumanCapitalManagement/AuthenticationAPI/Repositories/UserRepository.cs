using AuthenticationAPI.Models;

namespace AuthenticationAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Id = 1, Username = "Pesho", Firstname = "Petar", Lastname = "Petrov", Department = "Management", Position = "Manager", Password = "12345", Salary = 50000, Role = "manager"},
            new User { Id = 2, Username = "Ivan",Firstname = "Ivan", Lastname = "Ivanov", Department = "IT", Position = "Dev", Password = "12345", Salary = 40000, Role = "employee"},
            new User { Id = 3, Username = "Maria",Firstname = "Maria", Lastname = "Marieva", Department = "HR", Position = "LocalHR", Password = "12345", Salary = 30000, Role = "hradmin"}
        };

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public User ValidateUser(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
