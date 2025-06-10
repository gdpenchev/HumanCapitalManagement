using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;
using PeopleManagementUI.Services;

namespace PeopleManagementUI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IApiClientFactory apiClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiClientFactory = apiClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _apiClientFactory.CreateAuthenticationClient();
            var resposne = await client.PostAsJsonAsync(_configuration["Endpoints:Authentication"], model);

            if (!resposne.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid credentials";
                return View(model);
            }

            var result = await resposne.Content.ReadFromJsonAsync<TokenResponse>();
            HttpContext.Session.SetString("token", result.Token);
            return RedirectToAction("Index", "Employee");
        }
    }
}
