using System.ComponentModel.DataAnnotations;

namespace DEMO.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        public string RoleDescription { get; set; }
        public DateTime created { get; set; } = DateTime.Now;

        public DateTime modified { get; set; } = DateTime.Now;

        public ICollection<User> users { get; set; } = new List<User>();  
    }
}
