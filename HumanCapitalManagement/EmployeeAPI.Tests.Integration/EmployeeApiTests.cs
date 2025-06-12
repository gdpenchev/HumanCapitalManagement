using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EmployeeAPI.Tests.Integration
{
    public class EmployeeApiTests : IDisposable
    {
        private WebApplicationFactory<EmployeeAPI.Program> _employeeFactory;
        private WebApplicationFactory<AuthenticationAPI.Program> _authFactory;
        private HttpClient _employeeClient;
        private HttpClient _authClient;

        [SetUp]
        public void SetUp()
        {
            _employeeFactory = new WebApplicationFactory<EmployeeAPI.Program>();
            _employeeClient = _employeeFactory.CreateClient();

            _authFactory = new WebApplicationFactory<AuthenticationAPI.Program>();
            _authClient = _authFactory.CreateClient();
        }

        public void Dispose()
        {
            _employeeFactory?.Dispose();
            _authFactory?.Dispose();
        }
        [Test]
        public async Task Create_WithValidLoginModel_ReturnsSuccess()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            var employee = new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await _employeeClient.PostAsJsonAsync("https://localhost:7195/api/employee/create", employee);

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Does.Contain("Test").IgnoreCase);
        }

        [Test]
        public async Task GetById_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            int employeeId = 1;
            var response = await _employeeClient.GetAsync($"https://localhost:7195/api/employee/getById/{employeeId}");

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Does.Contain("Petar").IgnoreCase);
        }

        [Test]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            int employeeId = 999;
            var response = await _employeeClient.GetAsync($"https://localhost:7195/api/employee/getById/{employeeId}");

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(content, Does.Contain($"Employee with ID {employeeId} not found."));
        }

        [Test]
        public async Task GetAllEmployees_ReturnsSuccess()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await _employeeClient.GetAsync($"https://localhost:7195/api/employee/getall");

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var employees = doc.RootElement;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(employees.GetArrayLength(), Is.EqualTo(3));
        }

        [Test]
        public async Task Update_WithValidUser_ReturnsSuccess()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var employeeToUpdate = new Employee { FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            var employeeId = 1;
            //Act
            var response = await _employeeClient.PutAsJsonAsync($"https://localhost:7195/api/employee/update/{employeeId}", employeeToUpdate);

            var content = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Does.Contain("Test").IgnoreCase);
        }

        [Test]
        public async Task Update_WithInvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            var nullContent = new StringContent("null", Encoding.UTF8, "application/json");
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var employeeId = 1;
            //Act
            var response = await _employeeClient.PutAsJsonAsync($"https://localhost:7195/api/employee/update/{employeeId}", nullContent);

            var content = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsOk()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            var nullContent = new StringContent("null", Encoding.UTF8, "application/json");
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var employeeId = 1;
            //Act
            var response = await _employeeClient.DeleteAsync($"https://localhost:7195/api/employee/delete/{employeeId}");

            var content = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Does.Contain($"Employee with ID {employeeId} deleted.").IgnoreCase);
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            var nullContent = new StringContent("null", Encoding.UTF8, "application/json");
            _employeeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var employeeId = 999;
            //Act
            var response = await _employeeClient.DeleteAsync($"https://localhost:7195/api/employee/delete/{employeeId}");

            var content = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(content, Does.Contain($"Employee with ID {employeeId} not found.").IgnoreCase);
        }

        private async Task<string> GetJwtTokenAsync()
        {
            var credentials = new { username = "Maria", password = "12345" };

            var response = await _authClient.PostAsJsonAsync("https://localhost:7143/api/auth/login", credentials);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("token").GetString();
        }
    }
}
