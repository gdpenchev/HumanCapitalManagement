using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;

namespace PeopleManagementUI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
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
