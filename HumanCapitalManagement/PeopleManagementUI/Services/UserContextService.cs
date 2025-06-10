using System.IdentityModel.Tokens.Jwt;

namespace PeopleManagementUI.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Service for retrieving user context from JWT token.
        /// </summary>
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Gets the user's role from the JWT token stored in session.
        /// </summary>
        public string GetUserRole()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return "employee";
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                return jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "employee";
            }
            catch (Exception)
            {
                return "employee";
            }
        }
    }
}
