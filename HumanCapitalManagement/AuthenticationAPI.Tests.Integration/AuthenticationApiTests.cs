using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AuthenticationAPI.Tests.Integration
{
    public class AuthenticationApiTests : IDisposable
    {
        private WebApplicationFactory<AuthenticationAPI.Program> _factory;
        private HttpClient _client;

        public void Dispose()
        {
           _factory?.Dispose();
           _client?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<AuthenticationAPI.Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Login_WithValidCredentials_ReturnsSuccess()
        {
            //Arrange
            var credentials = new { username = "Maria", password = "12345" };

            //Act
            var response = await _client.PostAsJsonAsync("https://localhost:7143/api/auth/login", credentials);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.That(content, Does.Contain("token").IgnoreCase);
        }
        [Test]
        public async Task Login_WithNullLoginModel_ReturnsBadRequest()
        {
            //Act
            var content = new StringContent("null", Encoding.UTF8, "application/json");
            var response = await _client.PostAsJsonAsync("https://localhost:7143/api/auth/login", content);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task Login_UserValidationFail_ReturnsUnauthorized()
        {
            //Arrange
            var credentials = new { username = "Test", password = "test" };
            //Act
            var response = await _client.PostAsJsonAsync("https://localhost:7143/api/auth/login", credentials);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
