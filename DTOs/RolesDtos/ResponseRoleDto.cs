
using DEMO.DTOs.UsersDtos;

namespace DEMO.DTOs.RolesDtos
{
    public class ResponseRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string RoleDescription { get; set; } = string.Empty;

    }
}
