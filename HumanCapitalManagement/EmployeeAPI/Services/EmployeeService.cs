using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new()
        {
            new Employee { Id = 1, FirstName = "Petar", LastName = "Petrov", Department = "IT", Position = "QA",  Salary = 45000},
            new Employee { Id = 2, FirstName = "Ivan", LastName = "Ivanov", Department = "IT", Position = "Dev", Salary = 40000},
            new Employee { Id = 3, FirstName = "Gloria", LastName = "Ivanova", Department = "Sales", Position = "Account Manager", Salary = 30000}
        };
        public Employee Create(Employee employee)
        {
            employee.Id = _employees.Any() ? _employees.Max(e => e.Id) + 1 : 1;
            _employees.Add(employee);
            Console.WriteLine($"Created employee: {employee.FirstName} {employee.LastName} (ID: {employee.Id})");
            return employee;
        }

        public bool Delete(int employeeId)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee is null)
            {
                Console.WriteLine($"Delete failed: Employee ID {employeeId} not found.");
                return false;
            }

            _employees.Remove(employee);
            Console.WriteLine($"Deleted employee: {employee.FirstName} {employee.LastName} (ID: {employeeId})");
            return true;
        }

        public IEnumerable<Employee> GetAllEmployees() => _employees;

        public Employee GetById(int id)
        {
            var employee = _employees.FirstOrDefault(emp => emp.Id == id);
            if (employee is null)
            {
                Console.WriteLine($"GetById failed: Employee ID {id} not found.");
            }
            return employee;
        }

        public Employee Update(int employeeId, Employee updatedEmployee, string role)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee is null)
            {
                Console.WriteLine($"Update failed: Employee ID {employeeId} not found.");
                return null;
            }

            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Position = updatedEmployee.Position;
            employee.Department = updatedEmployee.Department;

            if (role is "hradmin") employee.Salary = updatedEmployee.Salary;

            Console.WriteLine($"Updated employee: {employee.FirstName} {employee.LastName} (ID: {employeeId}) by role: {role}");

            return employee;
        }
    }
}
