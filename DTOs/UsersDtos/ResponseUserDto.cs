using DEMO.DTOs.RolesDtos;
using DEMO.Models;

namespace DEMO.DTOs.UsersDtos
{
    public class ResponseUserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;
    }
}
