using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class AuthRegisterDto : IAuthRegisterDto
    {
        public string? UserName {get;set;}
        public string? Password {get;set;}
        public string? Email {get;set;}
    }
}