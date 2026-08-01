namespace DEMO.DTOs.UsersDtos
{
    public class UpdatedUserDto
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
