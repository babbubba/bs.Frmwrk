namespace bs.Frmwrk.Core.Dtos.Auth
{
    public interface IChangeUserPasswordDto
    {
        string UserName { get; set; }
        string Password { get; set; }
        string PasswordConfirm { get; set; }
        string? OldPassword { get; set; }
        string? RecoveryPasswordId { get; set; }

    }
}