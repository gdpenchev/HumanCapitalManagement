using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new()
        {
            new Employee { Id = 1, FirstName = "Petar", LastName = "Petrov", Department = "Management", Position = "Manager",  Salary = 50000},
            new Employee { Id = 2, FirstName = "Ivan", LastName = "Ivanov", Department = "IT", Position = "Dev", Salary = 40000},
            new Employee { Id = 3, FirstName = "Maria", LastName = "Marieva", Department = "HR", Position = "LocalHR", Salary = 30000}
        };
        public Employee Create(Employee employee)
        {
            employee.Id = _employees.Count + 1;
            _employees.Add(employee);

            return employee;
        }

        public bool Delete(int employeeId)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee is null) return false;

            _employees.Remove(employee);

            return true;
        }

        public IEnumerable<Employee> GetAllEmployees() => _employees;

        public Employee Update(int employeeId, Employee updatedEmployee, string role)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee is null) return null;

            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Position = updatedEmployee.Position;
            employee.Department = updatedEmployee.Department;

            if (role is "hradmin") employee.Salary = updatedEmployee.Salary;

            return employee;
        }
    }
}
