using bs.Frmwrk.Core.Dtos.Auth;
using System.Security.Claims;

namespace bs.Frmwrk.Core.Services.Auth
{
    public interface ITokenService
    {
        /// <summary>
        /// Gets the principal from expired token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        /// <summary>
        /// Generates the refresh token.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        /// <summary>
        /// Generates the access token.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        ITokenJWTDto GenerateAccessToken(IEnumerable<Claim> claims);
        /// <summary>
        /// Generates the refresh token expire date.
        /// </summary>
        /// <returns></returns>
        DateTime GenerateRefreshTokenExpireDate();
        /// <summary>
        /// Generates the access token expire date.
        /// </summary>
        /// <returns></returns>
        DateTime GenerateAccessTokenExpireDate();
    }
}