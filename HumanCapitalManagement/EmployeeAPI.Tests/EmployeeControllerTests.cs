using EmployeeAPI.Controllers;
using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EmployeeAPI.Tests
{
    public class EmployeeControllerTests : IDisposable
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private EmployeeController _controller;

        public void Dispose()
        {
            _controller?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Test]
        public void GetAllEmployees_ReturnsOkResult()
        {
            //Arrange
            _employeeServiceMock.Setup(s => s.GetAllEmployees()).Returns(new List<Employee> { new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 } });

            //Act
            var result = _controller.GetAllEmployees() as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            _employeeServiceMock.Verify(s => s.GetAllEmployees(), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }
        [Test]
        public void GetById_ReturnsOkResult()
        {
            //Arrange
            var employee = new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            _employeeServiceMock.Setup(s => s.GetById(1)).Returns(employee);

            //Act
            var result = _controller.GetById(1) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            _employeeServiceMock.Verify(s => s.GetById(1), Times.Once);
            Assert.That((result.Value as Employee).FirstName, Is.EqualTo("Test"));
        }
        [Test]
        public void GetById_ReturnsNotFoundResult()
        {
            //Arrange
            _employeeServiceMock.Setup(s => s.GetById(9999)).Returns((Employee)null);

            //Act
            var result = _controller.GetById(9999);

            //Assert
            Assert.IsNotNull(result);
            _employeeServiceMock.Verify(s => s.GetById(9999), Times.Once);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test]
        public void Create_ValidEmployee_ReturnsOk()
        {
            //Arrange
            var employee = new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            _employeeServiceMock.Setup(s=>s.Create(employee)).Returns(employee);
            
            //Act
            var result = _controller.Create(employee) as OkObjectResult;

            Assert.IsNotNull(result);
            _employeeServiceMock.Verify(s => s.Create(employee), Times.Once);
            Assert.That((result.Value as Employee).FirstName, Is.EqualTo(employee.FirstName));
        }
        [Test]
        public void Update_NonExistingId_ReturnsNotFound()
        {
            //Arrange
            var employee = new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "hradmin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _employeeServiceMock.Setup(s => s.GetById(100)).Returns((Employee)null);
            //Act
            var result = _controller.Update(100, employee);
            //Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            _employeeServiceMock.Verify(s => s.GetById(100), Times.Never);
        }
        [Test]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            //Act
            var result = _controller.Delete(99);
            //Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public void Delete_ExistingId_ReturnsOk()
        {
            //Assert
            _employeeServiceMock.Setup(s => s.Delete(1)).Returns(true);
            //Act
            var result = _controller.Delete(1) as OkObjectResult;

            //Assert
            Assert.That(result.Value, Is.EqualTo($"Employee with ID {1} deleted."));
            _employeeServiceMock.Verify(s => s.Delete(1), Times.Once);
        }

    }
}
