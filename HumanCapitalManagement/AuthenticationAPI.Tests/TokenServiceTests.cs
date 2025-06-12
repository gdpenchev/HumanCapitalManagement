using NUnit.Framework;
using AuthenticationAPI.Models;
using AuthenticationAPI.Services;
using Microsoft.Extensions.Configuration;

namespace AuthenticationAPI.Tests
{
    public class TokenServiceTests
    {
        [Test]
        public void GenerateJwtToken_Should_Return_Token()
        {
            // Arrange
            var user = new User { Username = "TestUser", Role = "admin" };
            var config = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string> { { "Jwt:Key", "verysecretkey1234567890987654321" } }).Build();
            var service = new TokenService(config);

            // Act
            var token = service.GenerateJwtToken(user);

            // Assert
            Assert.IsNotNull(token);
            Assert.That(token, Is.Not.Empty);
        }
    }
}
