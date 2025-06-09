using AuthenticationAPI.Models;
using AuthenticationAPI.Repositories;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserRepository userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var user = _userRepository.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (user == null)
                return Unauthorized();

            var token = _tokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }
}
