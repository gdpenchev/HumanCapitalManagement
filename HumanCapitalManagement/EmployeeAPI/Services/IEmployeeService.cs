using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee Create(Employee employee);
        Employee Update(int employeeId, Employee updatedEmployee);
        bool Delete(int employeeId);
    }
}
