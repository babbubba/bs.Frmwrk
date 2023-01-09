namespace bs.Frmwrk.Core.Dtos.Auth
{
    public interface IRequestRecoveryUserPasswordLinkDto
    {
        string UserName { get; set; }
        string Email { get; set; }

    }
}