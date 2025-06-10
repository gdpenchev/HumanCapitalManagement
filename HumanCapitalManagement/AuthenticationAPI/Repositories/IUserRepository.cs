using AuthenticationAPI.Models;

namespace AuthenticationAPI.Repositories
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);
    }
}
