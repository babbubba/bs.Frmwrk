using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Test.Dtos
{
    public class AuthRequestDto : IAuthRequestDto
    {
        public AuthRequestDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}