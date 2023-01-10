using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class CreateUserDto : ICreateUserDto
    {
        public CreateUserDto(string userName, string password, string[]? rolesIds)
        {
            UserName = userName;
            Password = password;
            RolesIds = rolesIds;
        }

        public string Email { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public string Password { get; set; }
        public string[]? RolesIds { get; set; }
        public string UserName { get; set; }
    }
}