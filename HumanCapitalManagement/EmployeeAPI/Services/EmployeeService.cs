using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new();
        public Employee Create(Employee employee)
        {
            employee.Id = _employees.Count + 1;
            _employees.Add(employee);

            return employee;
        }

        public bool Delete(int employeeId)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee != null)
                return false;

            _employees.Remove(employee);

            return true;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public Employee Update(int employeeId, Employee updatedEmployee)
        {
            var employee = _employees.FirstOrDefault(em => em.Id == employeeId);
            if (employee != null)
                return null;

            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;
            employee.Email = updatedEmployee.Email;
            employee.Position = updatedEmployee.Position;
            employee.Department = updatedEmployee.Department;

            return employee;
        }
    }
}
