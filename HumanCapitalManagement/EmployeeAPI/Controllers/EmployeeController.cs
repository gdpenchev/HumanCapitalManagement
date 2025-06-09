using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/people")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult Create(Employee emplyee)
        {
            if (emplyee is null)
                return BadRequest("Employee data is required.");

            return Ok(_employeeService.Create(emplyee));
            
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_employeeService.GetAllEmployees());
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Employee emplyee)
        {
            if (emplyee is null)
                return BadRequest("Employee data is required.");
            
            return Ok(_employeeService.Update(id, emplyee));
        }
        [HttpDelete("{id")]
        public IActionResult Delete(int id)
        {
            return Ok(_employeeService.Delete(id));
        }
    }
}
