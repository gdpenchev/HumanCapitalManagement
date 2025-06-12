using AuthenticationAPI.Controllers;
using AuthenticationAPI.Models;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuthenticationAPI.Tests
{
    public class AuthenticationControllerTests : IDisposable
    {
        private AuthenticationController _controller;
        private Mock<IUserService> _userServiceMock;
        private Mock<ITokenService> _tokenServiceMock;

        public void Dispose()
        {
            _controller?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _controller = new AuthenticationController(_userServiceMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public void Login_NullRequest_ReturnsBadRequest()
        {
            //Act
            var result = _controller.Login(null);

            //Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            //Arrange
            _userServiceMock.Setup(s => s.ValidateUser("test", "pass")).Returns((Models.User)null);
            //Act
            var result = _controller.Login(new LoginRequest { Username = "test", Password = "pass" });
            //Assert
            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
            _userServiceMock.Verify(s => s.ValidateUser("test", "pass"), Times.Once());
        }
        [Test]
        public void Login_ValidCredentials_ReturnsToken()
        {
            //Arrange
            var user = new Models.User { Username ="Maria", Password = "12345", Role="hradmin" };
            _userServiceMock.Setup(s=>s.ValidateUser("Maria","12345")).Returns(user);
            _tokenServiceMock.Setup(t => t.GenerateJwtToken(user)).Returns("token");
            //Act
            var result = _controller.Login(new LoginRequest { Username = "Maria", Password = "12345" }) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Value.ToString(), Does.Contain("token"));
            _userServiceMock.Verify(s => s.ValidateUser("Maria", "12345"), Times.Once());
            _tokenServiceMock.Verify(t => t.GenerateJwtToken(user), Times.Once());
        }
    }
}
