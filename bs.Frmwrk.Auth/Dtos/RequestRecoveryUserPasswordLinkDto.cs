using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class RequestRecoveryUserPasswordLinkDto : IRequestRecoveryUserPasswordLinkDto
    {
        public string Email { get; set; }
    }
}