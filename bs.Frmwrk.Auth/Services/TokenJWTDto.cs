using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Services
{
    public class TokenJWTDto : ITokenJWTDto
    {
        public TokenJWTDto(string tokenString, DateTime expireAt)
        {
            Token = tokenString;
            ExpireAt = expireAt;
        }

        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}