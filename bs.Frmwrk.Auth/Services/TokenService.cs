using AutoMapper;
using bs.Data.Interfaces;
using bs.Frmwrk.Base;
using bs.Frmwrk.Base.Exceptions;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Security;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Auth.Services
{
    public class TokenService : BsService, ITokenService
    {
        private readonly ISecuritySettings securitySettings;

        public TokenService(ILogger<TokenService> logger, ITranslateService translateService, IMapper mapper, IUnitOfWork unitOfWork, ISecurityService securityService,
            ISecuritySettings securitySettings) : base(logger, translateService, mapper, unitOfWork, securityService)
        {
            this.securitySettings = securitySettings;
        }

        public ITokenJWTDto GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var expireAt = GenerateAccessTokenExpireDate();

            var tokeOptions = new JwtSecurityToken(
                issuer: securitySettings.ValidTokenIssuer,
                audience: securitySettings.ValidTokenAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: GetSigningCredential()
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new TokenJWTDto(tokenString, expireAt);
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public DateTime GenerateRefreshTokenExpireDate()
        {
            return DateTime.UtcNow.AddMinutes(securitySettings.JwtRefreshTokenValidityMinutes ?? 30);

        }

        public virtual ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = securitySettings.ValidateIssuer?? false,
                ValidateAudience = securitySettings.ValidateAudience ?? false,
                ValidAudience = securitySettings.ValidTokenAudience,
                ValidIssuer = securitySettings.ValidTokenIssuer,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = GetSecurityKey(),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(GetSecurityAlgorithms(), StringComparison.InvariantCultureIgnoreCase))
                throw new BsException(2212042336, T("L'algoritmo di sicurezza del token non è valido."));
            return principal;
        }




        public DateTime GenerateAccessTokenExpireDate()
        {
            return DateTime.UtcNow.AddMinutes(securitySettings.JwtTokenValidityMinutes ?? 5);
        }
        /// <summary>
        /// Gets the secret key.
        /// </summary>
        /// <returns></returns>
        private byte[] GetSecretKey()
        {
            if(securitySettings.Secret is null)
            {
                throw new BsException(2212042341, T("Configuration error: 'Invalid secret key'."));
            }
            return Encoding.UTF8.GetBytes(securitySettings.Secret);
        }

        /// <summary>
        /// Gets the security key.
        /// </summary>
        /// <returns></returns>
        private SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(GetSecretKey());
        }

        /// <summary>
        /// Gets the signing credential.
        /// </summary>
        /// <returns></returns>
        private SigningCredentials GetSigningCredential()
        {
            return new SigningCredentials(GetSecurityKey(), GetSecurityAlgorithms());
        }

        /// <summary>
        /// Gets the security algorithms.
        /// </summary>
        /// <returns></returns>
        private static string GetSecurityAlgorithms()
        {
            return SecurityAlgorithms.HmacSha256;
        }










    }
}
