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

        [Authorize(Roles = "hradmin")]
        [HttpPost("create")]
        public IActionResult Create([FromBody]Employee emplyee)
        {
            if (emplyee is null) return BadRequest("Employee data is required.");

            return Ok(_employeeService.Create(emplyee));
            
        }

        [Authorize(Roles = "hradmin,manager,employee")]
        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value ?? "employee";
            var employee = _employeeService.GetAllEmployees().FirstOrDefault(em => em.Id == id);
            return Ok(employee);
        }

        [Authorize(Roles = "hradmin,manager,employee")]
        [HttpGet("getall")]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();

            if (employees == null) return BadRequest("No employee records.");

            return Ok(employees);
        }

        [Authorize(Roles = "hradmin,manager")]
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, Employee employee)
        {
            if (employee is null) return BadRequest("Employee data is required.");

            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "employee";

            return Ok(_employeeService.Update(id, employee, role));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_employeeService.Delete(id));
        }
    }
}
