using System.ComponentModel.DataAnnotations;

namespace DEMO.DTOs.RolesDtos
{
    public class CreatedRoleDto
    {
        [Required]
        [StringLength(20)]
        public string RoleName { get; set; }

        [Required]
        [StringLength (100)]
        public string RoleDescription { get; set; }

    }
}
