using DEMO.Data;
using DEMO.DTOs.RolesDtos;
using DEMO.DTOs.UsersDtos;
using DEMO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RoleController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Get_All_Roles
        [HttpGet]
        public async Task<ActionResult<List<ResponseRoleDto>>> GetAllRoles()
        {
            var roles = await _context.Roles
                .Select(r => new ResponseRoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    RoleDescription = r.RoleDescription,
                })
                .ToListAsync();
            if (roles == null || roles.Count == 0)
            {
                return NotFound();
            }
            
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseRoleDto>> CreateRole(CreatedRoleDto createdRoleDto)
        {
            var existingRole = await _context.Roles
            .AnyAsync(r => r.RoleName == createdRoleDto.RoleName);
            if (existingRole)
            {
                return BadRequest("Role already exists.");
            }
            var role = new Role
            {
                RoleName = createdRoleDto.RoleName,
                RoleDescription = createdRoleDto.RoleDescription,
                created = DateTime.Now,
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return Created();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseRoleDto>> GetRoleById(int id)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var dto = new ResponseRoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription
            };

            return Ok(dto);
        }

        // Update_Role
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseRoleDto>> UpdateRole(int id, UpdatedRoleDto updatedRoleDto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            role.RoleName = updatedRoleDto.RoleName;
            role.RoleDescription = updatedRoleDto.RoleDescription;
            role.modified = DateTime.Now;
            await _context.SaveChangesAsync();
            var dto = new ResponseRoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription
            };
            return Ok(dto);
        }


        // Delete_Role
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
