using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class TokenJWTDto : ITokenJWTDto
    {
        public TokenJWTDto(string tokenString, DateTime expireAt)
        {
            Token = tokenString;
            ExpireAt = expireAt;
        }

        public DateTime ExpireAt { get; set; }
        public string Token { get; set; }
    }
}