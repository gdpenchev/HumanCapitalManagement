﻿using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetById(int id);
        Employee Create(Employee employee);
        Employee Update(int employeeId, Employee updatedEmployee, string role);
        bool Delete(int employeeId);
    }
}
