using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeNo { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
