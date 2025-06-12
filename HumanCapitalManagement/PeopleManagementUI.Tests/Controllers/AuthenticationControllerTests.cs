using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using PeopleManagementUI.Controllers;
using PeopleManagementUI.Models;
using PeopleManagementUI.Services;
using System.Net;
using System.Net.Http.Json;

namespace PeopleManagementUI.Tests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests : IDisposable
    {
        private Mock<IApiClientFactory> _apiClientFactoryMock;
        private Mock<IConfiguration> _configurationMock;
        private AuthenticationController _controller;
        private Mock<HttpContext> _httpContextMock;
        private Mock<HttpMessageHandler> _handlerMock;
        private Mock<ISession> _sessionMock;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _apiClientFactoryMock = new Mock<IApiClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _httpContextMock = new Mock<HttpContext>();
            _handlerMock = new Mock<HttpMessageHandler>();
            _sessionMock = new Mock<ISession>();
            _httpClient = new HttpClient();

            _httpContextMock.Setup(c => c.Session).Returns(_sessionMock.Object);

            _controller = new AuthenticationController(_apiClientFactoryMock.Object, _configurationMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock.Object
                }
            };

            _configurationMock.Setup(c => c["Endpoints:Authentication"]).Returns("login");

        }
        [Test]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();
            // Assert
            var viewResult = (ViewResult)result;
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.IsNull(viewResult.ViewName);
        }
        [Test]
        public async Task Login_InvalidModelState_ReturnsViewWithError()
        {
            //Arrange
            var loginModel = new LoginViewModel { Username = "", Password = "" };
            _controller.ModelState.AddModelError("Username", "Required");
            //Act
            var result = await _controller.Login(loginModel);
            //Assert
            Assert.That(result,Is.TypeOf<ViewResult>());
            Assert.AreEqual("Please provide valid username and password.", _controller.ViewBag.Error);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            var loginModel = new LoginViewModel { Username = "test", Password = "pass" };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            var client = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            _apiClientFactoryMock.Setup(f => f.CreateAuthenticationClient()).Returns(client);

            // Act
            var result = await _controller.Login(loginModel);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;
            Assert.That(viewResult.Model, Is.SameAs(loginModel));
            Assert.AreEqual("Invalid credentials.", _controller.ViewBag.Error);
        }

        [Test]
        public async Task Login_ValidCredentials_SetsTokenAndRedirects()
        {
            //Arrange
            var loginModel = new LoginViewModel { Username = "test", Password = "pass" };
            var tokenResponse = new TokenResponse { Token = "jwt-token" };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(tokenResponse)
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var client = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            _apiClientFactoryMock.Setup(f => f.CreateAuthenticationClient()).Returns(client);

            // Act
            var result = await _controller.Login(loginModel);
            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}
