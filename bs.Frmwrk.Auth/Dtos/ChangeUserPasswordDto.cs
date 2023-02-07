using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class ChangeUserPasswordDto : IChangeUserPasswordDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string? RecoveryPasswordId { get; set; }
        public string? OldPassword { get; set; }
    }
}