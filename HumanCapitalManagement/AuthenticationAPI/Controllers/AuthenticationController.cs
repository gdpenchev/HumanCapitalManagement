using AuthenticationAPI.Models;
using AuthenticationAPI.Repositories;
using AuthenticationAPI.Services;
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
            if (loginRequest is null) return BadRequest("Login data is not full");

            var user = _userRepository.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (user == null) return Unauthorized();

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { token });
        }
    }
}
