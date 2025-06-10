using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace PeopleManagementUI.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EmployeeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<IActionResult> Index()
        {
            var client = GetAuthenticatedClient();
            //var role = GetUserRole();
            //ViewBag.Role = role;

            var resposne = await client.GetAsync(_configuration["Endpoints:EmployeeGetAll"]);
            if(!resposne.IsSuccessStatusCode)
                return Unauthorized();

            var employees = await resposne.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            //foreach (var em in employees)
            //    em.Role = role;
            return View(employees);
        }

        public IActionResult Create() => View(new EmployeeViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            var client = GetAuthenticatedClient();
            Console.WriteLine(JsonSerializer.Serialize(employeeViewModel));
            var response = await client.PostAsJsonAsync(_configuration["Endpoints:EmployeeCreate"], employeeViewModel);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = GetAuthenticatedClient();
            var response = await client.GetAsync(_configuration["Endpoints:EmployeeGetAll"]);
            var list = await response.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            var employee = list.FirstOrDefault(em => em.Id == id);
            //employee.Role = UserRole();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
            var client = GetAuthenticatedClient();
            var resp = await client.PutAsJsonAsync(_configuration["Endpoints:EmployeeUpdate"] + "/" + employeeViewModel.Id, employeeViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = GetAuthenticatedClient();
            var respons = await client.DeleteAsync(_configuration["Endpoints:EmployeeDelete"] + "/" + id);
            return RedirectToAction("Index");
        }

        private HttpClient GetAuthenticatedClient()
        {
            var token = HttpContext.Session.GetString("token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        //private string GetUserRole()
        //{
        //    var token = HttpContext.Session.GetString("token");
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwt = handler.ReadJwtToken(token);
        //    return jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "employee";
        //}
    }
}
