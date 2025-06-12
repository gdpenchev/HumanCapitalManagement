using EmployeeAPI.Models;
using EmployeeAPI.Services;
namespace EmployeeAPI.Tests
{
    public class EmployeeServiceTests
    {
        private IEmployeeService _employeeService;

        [SetUp]
        public void Setup()
        {
            _employeeService = new EmployeeService();
        }
        [Test]
        public void GetAllEmployees_ReturnsALlEmployeesSuccessfully()
        {
            //Act
            var employees = _employeeService.GetAllEmployees();
            //Assert
            Assert.IsNotNull(employees);
            Assert.AreEqual(employees.Count(), 3);
        }
        [Test]
        public void GetById_ValidId_ReturnsEmployeeSuccessfully()
        {
            //Act
            var employee = _employeeService.GetById(1);
            //Assert
            Assert.IsNotNull(employee);
            Assert.That(employee.FirstName, Is.EqualTo("Petar"));
        }
        [Test]
        public void GetById_InvalidId_ReturnsNull()
        {
            //Act
            var employee = _employeeService.GetById(999);
            //Assert
            Assert.IsNull(employee);
        }
        [Test]
        public void CreateEmployee_CreatesEmployeeSuccessfully()
        {
            //Arrange
            var newEmployee = _employeeService.Create(new Employee { Id = 4, FirstName = "Stoyan", LastName = "Stoyanov", Department = "Dev", Position = "IT", Salary = 123456 });
            //Act
            var result = _employeeService.GetById(4);
            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo(newEmployee.FirstName));
        }
        [Test]
        public void UpdateEmployee_ValidId_UpdatesSuccessfully()
        {
            //Arrange
            var updatedEmployee = new Employee { Id = 1, FirstName = "Test", LastName = "Test2", Department = "NewDepartment", Position = "Sales", Salary = 4567778 };
            _employeeService.Update(1, updatedEmployee, "hradmin");
            //Act
            var result = _employeeService.GetById(1);
            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo(updatedEmployee.FirstName));
        }
        [Test]
        public void DeleteEmployee_SuccessfullyRemovesEmployee()
        {
            //Arrange
            _employeeService.Delete(1);
            //Act
            var result = _employeeService.GetById(1);
            //Assert
            Assert.IsNull(result);
        }
    }
}
