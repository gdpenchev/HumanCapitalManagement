using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        /// <summary>
        /// Creates a new employee (HR Admin only).
        /// </summary>
        [Authorize(Roles = "hradmin")]
        [HttpPost("create")]
        public IActionResult Create(Employee employee)
        {
            if (employee is null)
            {
                Console.WriteLine("Create failed: Employee data is null.");
                return BadRequest(new { Message = "Employee data is required." });
            }

            try
            {
                var createdEmployee = _employeeService.Create(employee);
                return Ok(createdEmployee);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Create failed: {ex.Message}");
                return BadRequest(new { Message = ex.Message.ToString() });
            }

        }
        /// <summary>
        /// Gets an employee by ID (HR Admin and Manager only)
        /// </summary>
        [Authorize(Roles = "hradmin,manager")]
        [HttpGet("getById/{id}")]
        public IActionResult GetById(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee is null)
            {
                Console.WriteLine($"GetById failed: Employee ID {id} not found.");
                return NotFound($"Employee with ID {id} not found.");
            };
            return Ok(employee);
        }
        /// <summary>
        /// Gets all employees.
        /// </summary>
        [Authorize(Roles = "hradmin,manager,employee")]
        [HttpGet("getAll")]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }
        /// <summary>
        /// Updates an employee by ID (HR Admin or Manager only).
        /// </summary>
        [Authorize(Roles = "hradmin,manager")]
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, Employee employee)
        {
            if (employee is null)
            {
                Console.WriteLine("Update failed: Employee data is null.");
                return BadRequest(new { Message = "Employee data is required." });
            }

            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "employee";

            try
            {
                var updatedEmployee = _employeeService.Update(id, employee, role);
                if (updatedEmployee is null)
                {
                    Console.WriteLine($"Update failed: Employee ID {id} not found.");
                    return NotFound(new { Message = $"Employee with ID {id} not found." });
                }

                return Ok(updatedEmployee);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                return BadRequest(new { Message = ex.Message.ToString() });
            }
        }
        /// <summary>
        /// Deletes an employee by ID (HR Admin only).
        /// </summary>
        [Authorize(Roles = "hradmin")]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _employeeService.Delete(id);
            if (!deleted)
            {
                Console.WriteLine($"Delete failed: Employee ID {id} not found.");
                return NotFound(new { Message = $"Employee with ID {id} not found." });
            }

            return Ok($"Employee with ID {id} deleted.");
        }
    }
}
