using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class ConfirmEmailDto : IConfirmEmailDto
    {
        public string UserId { get; set; }
      
        public string ConfirmationId { get; set; }
    }
}