using System.ComponentModel.DataAnnotations;

namespace PeopleManagementUI.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative number.")]
        public decimal Salary { get; set; } 
    }
}
