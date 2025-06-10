using Microsoft.AspNetCore.Mvc;
using PeopleManagementUI.Models;
using PeopleManagementUI.Services;
using System.Text.Json;

namespace PeopleManagementUI.Controllers
{
    /// <summary>
    /// Handles employee management operations.
    /// </summary>
    public class EmployeeController : BaseController
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IConfiguration _configuration;

        public EmployeeController(IApiClientFactory apiClientFactory, IConfiguration configuration, IUserContextService userContextService)
            : base(userContextService)
        {
            _configuration = configuration;
            _apiClientFactory = apiClientFactory;
        }
        /// <summary>
        /// Displays a list of all employees.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            var response = await client.GetAsync(_configuration["Endpoints:EmployeeGetAll"]);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"GetAllEmployees failed: {response.StatusCode}");
                TempData["Error"] = "Unable to load employees. Please try again.";
                return RedirectToAction("Login", "Authentication");
            }

            var employees = await response.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            //ViewData["UserRole"] = GetUserRole();
            return View(employees);
        }
        /// <summary>
        /// Displays the create employee form.
        /// </summary>
        public IActionResult Create()
        {
            //ViewData["UserRole"] = GetUserRole();
            return View(new EmployeeViewModel());
        }
        /// <summary>
        /// Creates a new employee.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(employeeViewModel.FirstName) || string.IsNullOrWhiteSpace(employeeViewModel.LastName))
            {
                Console.WriteLine("Create failed: Invalid employee data.");
                TempData["Error"] = "FirstName and LastName are required.";
                return View(employeeViewModel);
            }
            var client = _apiClientFactory.CreateEmployeeClient();
            Console.WriteLine($"Creating employee: {JsonSerializer.Serialize(employeeViewModel)}");
            var response = await client.PostAsJsonAsync(_configuration["Endpoints:EmployeeCreate"], employeeViewModel);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Create failed: {response.StatusCode}");
                TempData["Error"] = "Failed to create employee. Please try again.";
                return View(employeeViewModel);
            }

            return RedirectToAction("Index");

        }
        /// <summary>
        /// Displays the edit employee form.
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            var response = await client.GetAsync($"{_configuration["Endpoints:EmployeeGetById"]}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"GetById failed: {response.StatusCode}");
                TempData["Error"] = "Unable to load employee. Please try again.";
                return RedirectToAction("Index");
            }
            var employee = await response.Content.ReadFromJsonAsync<EmployeeViewModel>();
            if (employee is null)
            {
                Console.WriteLine($"GetById failed: Employee ID {id} not found.");
                TempData["Error"] = $"Employee with ID {id} not found.";
                return RedirectToAction("Index");
            }

            return View(employee);
        }
        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(employeeViewModel.FirstName) || string.IsNullOrWhiteSpace(employeeViewModel.LastName))
            {
                Console.WriteLine("Edit failed: Invalid employee data.");
                TempData["Error"] = "FirstName and LastName are required.";
                return View(employeeViewModel);
            }
            var client = _apiClientFactory.CreateEmployeeClient();
            Console.WriteLine($"Updating employee: {JsonSerializer.Serialize(employeeViewModel)}");

            var response = await client.PutAsJsonAsync($"{_configuration["Endpoints:EmployeeUpdate"]}/{employeeViewModel.Id}", employeeViewModel);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Edit failed: {response.StatusCode}");
                TempData["Error"] = "Failed to update employee. Please try again.";
                return View(employeeViewModel);
            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var client = _apiClientFactory.CreateEmployeeClient();
            Console.WriteLine($"Deleting employee ID: {id}");
            var response = await client.DeleteAsync($"{_configuration["Endpoints:EmployeeDelete"]}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Delete failed: {response.StatusCode}");
                TempData["Error"] = "Failed to delete employee. Please try again.";
            }

            return RedirectToAction("Index");
        }
    }
}
