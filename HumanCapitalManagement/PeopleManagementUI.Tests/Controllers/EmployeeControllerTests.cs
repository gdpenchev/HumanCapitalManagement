using PeopleManagementUI.Services;
using Microsoft.Extensions.Configuration;
using PeopleManagementUI.Controllers;
using Moq;
using PeopleManagementUI.Models;
using System.Net.Http.Json;
using System.Net;
using Moq.Protected;

namespace PeopleManagementUI.Tests.Controllers
{
    [TestFixture]
    public class EmployeeControllerTests : IDisposable
    {
        private Mock<IApiClientFactory> _apiClientFactoryMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private Mock<HttpMessageHandler> _httpHandlerMock;
        private HttpClient _httpClient;
        private EmployeeController _controller;

        public void Dispose()
        {
            _controller?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _apiClientFactoryMock = new Mock<IApiClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _httpHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            _httpClient = new HttpClient(_httpHandlerMock.Object);
            _apiClientFactoryMock.Setup(x => x.CreateEmployeeClient()).Returns(_httpClient);

            _configurationMock.Setup(x => x["Endpoints:EmployeeGetAll"]).Returns("http://test/api/employees");
            _configurationMock.Setup(x => x["Endpoints:EmployeeCreate"]).Returns("http://test/api/employees");

            _controller = new EmployeeController(_apiClientFactoryMock.Object, _configurationMock.Object, _userContextServiceMock.Object);
        }

        //[Test]
        //public async Task Index_ReturnsViewWithEmployees_WhenLoggedIn()
        //{
        //    //Arrange
        //    var employees = new List<EmployeeViewModel>
        //    {
        //        new EmployeeViewModel { Id = 1, FirstName = "Ivan", LastName = "Ivanov" },
        //        new EmployeeViewModel { Id = 2, FirstName = "Pesho", LastName = "Peshev" }
        //    };

        //    var response = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = JsonContent.Create(employees)
        //    };

        //    _httpHandlerMock.Protected()
        //        .Setup(HttpMethod.Get, "http://test/api/employees")
        //        .ReturnsAsync(response);
        //}
    }
}
