using System.ComponentModel.DataAnnotations;

namespace DEMO.DTOs.UsersDtos
{
    public class CreatedUsersDto
    {
        [Required]
        [StringLength(100)]
        public string UserName {  get; set; } = string.Empty;

        public int RoleId { get; set; } 

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [Length(6, 8)]
        public string Password { get; set; } = string.Empty;
    }
}
