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

        public string UserName{get;set;}
        public string Email{get;set;} = string.Empty;
        public string Password{get;set;}
        public bool Enabled { get; set; } = true;
        public string[]? RolesIds{get;set;}
    }
}