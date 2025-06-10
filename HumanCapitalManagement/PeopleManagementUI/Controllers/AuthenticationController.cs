using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;
using PeopleManagementUI.Services;

namespace PeopleManagementUI.Controllers
{
    /// <summary>
    /// Handles user authentication operations.
    /// </summary>
    public class AuthenticationController : Controller
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IApiClientFactory apiClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiClientFactory = apiClientFactory;
        }
        /// <summary>
        /// Displays the login view.
        /// </summary>
        [HttpGet]
        public IActionResult Login() => View();
        // <summary>
        /// Processes user login and stores JWT token in session.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Login failed: Invalid model state.");
                ViewBag.Error = "Please provide valid username and password.";
                return View(model);
            }

            var client = _apiClientFactory.CreateAuthenticationClient();
            var response = await client.PostAsJsonAsync(_configuration["Endpoints:Authentication"], model);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Login failed for username: {model.Username}");
                ViewBag.Error = "Invalid credentials.";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            HttpContext.Session.SetString("token", result.Token);
            Console.WriteLine($"Login successful for username: {model.Username}");
            return RedirectToAction("Index", "Employee");
        }
    }
}
