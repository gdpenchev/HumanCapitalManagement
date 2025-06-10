using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        [Authorize(Roles = "hradmin,manager")]
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
            //var role = User.FindFirst(ClaimTypes.Role).Value ?? "employee";
            return Ok(_employeeService.GetAllEmployees());
        }
        //[Authorize]
        //[HttpGet("{id}")]
        //public IActionResult GetEmployee(int id)
        //{
        //    var employee = _employeeService.GetAllEmployees().FirstOrDefault(emp=> emp.Id == id);
        //    if (employee is null) return NotFound();
        //    return Ok(employee);
        //}
        [Authorize(Roles = "hradmin,manager")]
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, Employee emplyee)
        {
            if (emplyee is null)
                return BadRequest("Employee data is required.");

            //var role = User.FindFirst(ClaimTypes.Role).Value ?? "employee";
            var existing = _employeeService.GetAllEmployees().FirstOrDefault(em => em.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            //if(role != "hradmin")
            //    emplyee.Salary = existing.Salary;
            return Ok(_employeeService.Update(id, emplyee));
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_employeeService.Delete(id));
        }
    }
}
