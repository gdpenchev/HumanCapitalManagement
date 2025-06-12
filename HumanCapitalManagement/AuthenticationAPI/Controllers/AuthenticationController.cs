using AuthenticationAPI.Models;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IUserService userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }
        /// <summary>
        /// User validation and provides token
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (loginRequest is null) 
            {
                Console.WriteLine("Invalid login attempt: Missing login details.");
                return BadRequest("Login data is not full");
            } 
                
            var user = _userRepository.ValidateUser(loginRequest.Username, loginRequest.Password);

            if (user == null)
            {
                Console.WriteLine($"Failed login attempt for username: {loginRequest.Username}");
                return Unauthorized();
            }

            Console.WriteLine($"Successful login for username: {loginRequest.Username}");
            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { token });
        }
    }
}
