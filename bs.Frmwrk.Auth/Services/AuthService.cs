using bs.Data.Interfaces;
using bs.Frmwrk.Auth.ViewModel;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Base.Services;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using bs.Frmwrk.Core.Services.Mapping;

namespace bs.Frmwrk.Auth.Services
{
    public class AuthService : BsService, IAuthService
    {
        //TODO: Implementa recupera password
        //TODO: Implementa cambia password
        //TODO: Implemnta registrazione (moderata)

        protected readonly IAuthRepository authRepository;
        private readonly ITokenService tokenService;

        public AuthService(ILogger<AuthService> logger, ITranslateService translateService, IMapperService mapper, IUnitOfWork unitOfWork,
            IAuthRepository authRepository, ITokenService tokenService, ISecurityService securityService) : base(logger, translateService, mapper, unitOfWork, securityService)
        {
            this.authRepository = authRepository;
            this.tokenService = tokenService;
        }

        public virtual async Task<IApiResponse<IUserViewModel>> AuthenticateAsync(IAuthRequestDto authRequest, string? clientIp)
        {
            return await ExecuteTransactionAsync<IUserViewModel>(async (response) =>
            {
                var user = await authRepository.GetUserByUserNameAsync(authRequest.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Accesso fallito");
                    logger.LogWarning($"Autentication error user: '{authRequest.UserName}' not found (IP: {clientIp ?? "*"})");
                    await securityService.TrackLoginFailAsync(authRequest.UserName, clientIp);
                    response.ErrorCode = 2212072057;
                    return response;
                }
                if (!CheckHashedPassword(user, authRequest.Password))
                {
                    response.Success = false;
                    response.ErrorMessage = T("Accesso fallito");
                    response.ErrorCode = 2212072057;
                    logger.LogWarning($"Autentication error for user: {authRequest.UserName} (IP: {clientIp ?? "*"})");
                    await securityService.TrackLoginFailAsync(authRequest.UserName, clientIp);
                    return response;
                }

                if (!user.Enabled)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Utente disabilitato");
                    response.ErrorCode = 2212072144;
                    logger.LogWarning($"Autentication blocked for disabled user: {authRequest.UserName} (IP: {clientIp ?? "*"})");
                    await securityService.TrackLoginFailAsync(authRequest.UserName, clientIp);
                    return response;
                }

                var token = GenerateClaimsAndToken(user);

                // generate a new refresh token
                user.RefreshToken = tokenService.GenerateRefreshToken();
                user.RefreshTokenExpire = tokenService.GenerateRefreshTokenExpireDate();

                user.LastLogin = DateTime.UtcNow;
                user.LastIp = clientIp;
                response.Value = mapper.Map<IUserViewModel>(user);
                response.Value.AccessToken = token.Token;

                return response;
            }, "Errore in autenticazione");
        }

        public virtual async Task<IApiResponse<string>> CreateUserAsync(ICreateUserDto createUserDto, IUserModel currentUser)
        {
            return await ExecuteTransactionAsync<string>(async (response) =>
            {
                var model = mapper.Map<IUserModel>(createUserDto);

                // Map the roles
                if (createUserDto.RolesIds != null && model is IRoledUser roledUser)
                {
                    foreach (var roleId in createUserDto.RolesIds)
                    {
                        roledUser.Roles.Add(await authRepository.GetRoleByIdAsync(roleId.ToGuid()));
                    }
                }

                if (!securityService.CheckPasswordValidity(createUserDto.Password, out string? passwordCheckingError))
                {
                    response.Success = false;
                    response.ErrorMessage = T("La password non soddisfa i criteri di sicurezza: '{0}'.", passwordCheckingError ?? "N/D");
                    response.ErrorCode = 2212042300;
                    return response;
                }

                model.PasswordHash = HashPassword(createUserDto.Password);

                await authRepository.CreateUserAsync(model);
                response.Value = model.Id.ToString();
                return response;
            }, "Errore creando l'utente");
        }

        public virtual async Task<IApiResponse> KeepAliveAsync(IKeepedAliveUser user)
        {
            return await ExecuteTransactionAsync(async (response) =>
            {
                if (user is not null)
                {
                    user.LastPing = DateTime.UtcNow;
                }

                return response;
            }, "Errore aggiornando lo stato dell'utente");
        }

        public virtual async Task<IApiResponse<IRefreshTokenViewModel>> RefreshAccessTokenAsync(IRefreshTokenRequestDto refreshTokenRequest)
        {
            return await ExecuteTransactionAsync<IRefreshTokenViewModel>(async (response) =>
            {
                var principal = tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Il refresh token non valido.");
                    response.ErrorCode = 202020000;
                    return response;
                }

                var user = await authRepository.GetUserByIdAsync(userId.ToGuid());
                if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpire <= DateTime.UtcNow)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Refresh token non valido o scaduto.");
                    response.ErrorCode = 202020001;
                    return response;
                }
                response.Value = new RefreshTokenViewModels
                {
                    AccessToken = tokenService.GenerateAccessToken(principal.Claims).Token,
                    RefreshToken = tokenService.GenerateRefreshToken(),
                    RefreshTokenExpire = tokenService.GenerateRefreshTokenExpireDate()
                };
                user.RefreshToken = response.Value.RefreshToken;
                user.RefreshTokenExpire = response.Value.RefreshTokenExpire;

                return response;
            }, "Errore durante il rinnovo del token");
        }

        private static bool CheckHashedPassword(IUserModel user, string clearPassword)
        {
            var passwordHash = user.PasswordHash;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(clearPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

        private ITokenJWTDto GenerateClaimsAndToken(IUserModel user)
        {
            if (user is null)
            {
                throw new BsException(2212072133, T("Impossibile generare Claims e Token. Utente non definito."));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("userName", user.UserName),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user is IRoledUser roledUser)
            {
                claims.AddRange(roledUser.Roles.Select(g => new Claim("role", g.Code)));
            }

            return tokenService.GenerateAccessToken(claims);
        }

        private static string HashPassword(string clearPassword)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(clearPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }
}