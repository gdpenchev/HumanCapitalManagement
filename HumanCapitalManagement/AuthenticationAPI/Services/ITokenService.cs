using AuthenticationAPI.Models;

namespace AuthenticationAPI.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
