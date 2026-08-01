using System.ComponentModel.DataAnnotations;

namespace DEMO.DTOs.UsersDtos
{
    public class CreatedUsersDto
    {
        public string UserName {  get; set; } = string.Empty;

        public int RoleId { get; set; } 

        public string EmailAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
