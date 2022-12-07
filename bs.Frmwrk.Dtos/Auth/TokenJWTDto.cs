using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Dtos;

namespace bs.Frmwrk.Dtos.Auth
{
    public  class TokenJWTDto : ITokenJWTDto
    {
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}