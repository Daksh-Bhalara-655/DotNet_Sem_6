using DEMO.Data;
using DEMO.DTOs.RolesDtos;
using DEMO.DTOs.UsersDtos;
using DEMO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _Context;
        public UserController(AppDbContext appDbContext)
        {
            _Context = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseUserDto>>> GetAll()
        {
            var users = await _Context.Users
                .Include(u => u.Role)
                .Select(u => new ResponseUserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    EmailAddress = u.EmailAddress,
                    RoleName = u.Role.RoleName
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<List<ResponseUserDto>>> CreateUser(CreatedUsersDto createdUsersDto)
        {
            var role = await _Context.Roles.FindAsync(createdUsersDto.RoleId);
            if (role == null)
            {
                return BadRequest("Invalid role ID.");
            }
            var user = new User
            {
                UserName = createdUsersDto.UserName,
                RoleId = createdUsersDto.RoleId,
                EmailAddress = createdUsersDto.EmailAddress,
                Password = createdUsersDto.Password,
                created = DateTime.Now,
            };
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
            return Created();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseUserDto>> GetById(int id)
        {
            var user = await _Context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = new ResponseUserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                RoleName = user.Role?.RoleName ?? string.Empty
            };

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            _Context.Users.Remove(user);
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        // Update_User
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseUserDto>> UpdateUser(int id, UpdatedUserDto updatedUserDto)
        {
            var user = await _Context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var role = await _Context.Roles.FindAsync(updatedUserDto.RoleId);
            if (role == null)
            {
                return BadRequest("Invalid role ID.");
            }
            user.UserName = updatedUserDto.UserName;
            user.RoleId = updatedUserDto.RoleId;
            user.EmailAddress = updatedUserDto.EmailAddress;
            user.Password = updatedUserDto.Password;
            user.modified = DateTime.Now;
            await _Context.SaveChangesAsync();
            var userDto = new ResponseUserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                RoleName = role.RoleName
            };
            return Ok(userDto);
        }

    }
}
