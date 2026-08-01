using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEMO.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
     
        public Role? Role { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [Length(6,8)]
        public string Password { get; set; } 

        [Required]
        public bool RememberMe { get; set; }  = true;

        public DateTime created { get; set; } = DateTime.Now;

        public DateTime modified { get; set; } = DateTime.Now;
    }
}
