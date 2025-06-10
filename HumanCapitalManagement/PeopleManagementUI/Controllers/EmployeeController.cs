using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;
using PeopleManagementUI.Services;
using System.Text.Json;

namespace PeopleManagementUI.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IConfiguration _configuration;

        public EmployeeController(IApiClientFactory apiClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiClientFactory = apiClientFactory;
        }
        
        public async Task<IActionResult> Index()
        {
            var client = _apiClientFactory.CreateEmployeeClient();

            var resposne = await client.GetAsync(_configuration["Endpoints:EmployeeGetAll"]);
            if(!resposne.IsSuccessStatusCode)
                return Unauthorized();

            var employees = await resposne.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            return View(employees);
        }

        public IActionResult Create() => View(new EmployeeViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            Console.WriteLine(JsonSerializer.Serialize(employeeViewModel));
            var response = await client.PostAsJsonAsync(_configuration["Endpoints:EmployeeCreate"], employeeViewModel);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            var response = await client.GetAsync(_configuration["Endpoints:EmployeeGetAll"]);
            var list = await response.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            var employee = list.FirstOrDefault(em => em.Id == id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            var resp = await client.PutAsJsonAsync(_configuration["Endpoints:EmployeeUpdate"] + "/" + employeeViewModel.Id, employeeViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            var respons = await client.DeleteAsync(_configuration["Endpoints:EmployeeDelete"] + "/" + id);
            return RedirectToAction("Index");
        }
    }
}
