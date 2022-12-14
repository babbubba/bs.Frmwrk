using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Test.Dtos
{
    public class AuthRequestDto : IAuthRequestDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}