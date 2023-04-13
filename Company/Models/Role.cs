using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
