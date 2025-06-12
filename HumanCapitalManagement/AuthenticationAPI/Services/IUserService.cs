using AuthenticationAPI.Models;

namespace AuthenticationAPI.Services
{
    public interface IUserService
    {
        User ValidateUser(string username, string password);
    }
}
