using bs.Data.Interfaces;
using bs.Frmwrk.Auth.Dtos;
using bs.Frmwrk.Auth.ViewModel;
using bs.Frmwrk.Base.Services;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace bs.Frmwrk.Auth.Services
{
    public class AuthService : BsService, IAuthService, IInitializableService
    {
        //TODO: Implemnta registrazione (moderata)
        public static int InitPriority => -10;

        protected readonly IAuthRepository authRepository;
        private readonly ISecuritySettings securitySettings;
        private readonly ITokenService tokenService;

        public AuthService(ILogger<AuthService> logger, ITranslateService translateService, IMapperService mapper, IUnitOfWork unitOfWork,
            IAuthRepository authRepository, ITokenService tokenService, ISecurityService securityService, ISecuritySettings securitySettings) : base(logger, translateService, mapper, unitOfWork, securityService)
        {
            this.authRepository = authRepository;
            this.tokenService = tokenService;
            this.securitySettings = securitySettings;
        }

        public event EventHandler<IAuthEventDto>? AuthEvent;

        public event EventHandler<IAuthEventDto>? LoginEvent;

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
                    OnLoginEvent(authRequest.UserName, false, T("Accesso fallito: utente non trovato"), clientIp);
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
                    OnLoginEvent(authRequest.UserName, false, T("Accesso fallito: password errata"), clientIp);
                    await securityService.TrackLoginFailAsync(authRequest.UserName, clientIp);
                    return response;
                }

                if (!user.Enabled)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Utente disabilitato");
                    response.ErrorCode = 2212072144;
                    logger.LogWarning($"Autentication blocked for disabled user: {authRequest.UserName} (IP: {clientIp ?? "*"})");
                    OnLoginEvent(authRequest.UserName, false, T("Accesso fallito: utente disabilitato"), clientIp);
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
                OnLoginEvent(authRequest.UserName, true, T("Accesso riuscito"), clientIp);

                return response;
            }, "Errore in autenticazione");
        }

        public async Task<IApiResponse> ChangePasswordAsync(IChangeUserPasswordDto changeUserPasswordDto, IUserModel? currentUser)
        {
            return await ExecuteTransactionAsync(async (response) =>
            {
                var query = unitOfWork.Session.Query<IUserModel>();
                query = query.Where(u => u.UserName == changeUserPasswordDto.UserName);

                if (currentUser is null && string.IsNullOrWhiteSpace(changeUserPasswordDto.RecoveryPasswordId))
                {
                    response.ErrorMessage = T("Impossibile cambiare la password (id ripristino non valido)");
                    response.ErrorCode = 2301091542;
                    response.Success = false;
                    return response;
                }

                if (currentUser is not null && string.IsNullOrWhiteSpace(changeUserPasswordDto.OldPassword))
                {
                    response.ErrorMessage = T("Impossibile cambiare la password (password attuale non valida)");
                    response.ErrorCode = 2301091548;
                    response.Success = false;
                    return response;
                }

                if (currentUser is null)
                {
                    query = query.Where(u => u.RecoveryPasswordId == changeUserPasswordDto.RecoveryPasswordId.ToGuid());
                }

                var user = await query.SingleOrDefaultAsync();
                if (user is null)
                {
                    response.ErrorMessage = T("Impossibile trovare l'utente o l'id per il ripristino della password");
                    response.ErrorCode = 2301091543;
                    response.Success = false;
                    return response;
                }

                if (changeUserPasswordDto.OldPassword is null || (currentUser is not null && !CheckHashedPassword(user, changeUserPasswordDto.OldPassword)))
                {
                    response.ErrorMessage = T("Impossibile cambiare la password (password attuale errata o non valida)");
                    response.ErrorCode = 2301091550;
                    response.Success = false;
                    return response;
                }

                if (changeUserPasswordDto.Password != changeUserPasswordDto.PasswordConfirm)
                {
                    response.ErrorMessage = T("Le password non coincidono");
                    response.ErrorCode = 2301091544;
                    response.Success = false;
                    return response;
                }

                if (!securityService.CheckPasswordValidity(changeUserPasswordDto.Password, out string? passwordCheckingError))
                {
                    response.Success = false;
                    response.ErrorMessage = T("La password non soddisfa i criteri di sicurezza: '{0}'.", passwordCheckingError ?? "N/D");
                    response.ErrorCode = 2301091545;
                    return response;
                }

                user.PasswordHash = HashPassword(changeUserPasswordDto.Password);

                await unitOfWork.Session.SaveAsync(user);
                return response;
            }, "Errore durante il cambiamento della password");
        }

        public virtual async Task<IApiResponse> ConfirmEmailAsync(IConfirmEmailDto confirmEmailDto)
        {
            return await ExecuteTransactionAsync(async (response) =>
            {
                var user = await unitOfWork.Session.GetAsync<IUserModel>(confirmEmailDto.UserId.ToGuid());
                if (user is null)
                {
                    response.ErrorMessage = T("Impossibile trovare l'utente richiesto");
                    response.ErrorCode = 2212221559;
                    response.Success = false;
                    return response;
                }

                if (user.ConfirmationId != confirmEmailDto.ConfirmationId.ToGuid())
                {
                    response.ErrorMessage = T("Link di conferma registrazione non valido");
                    response.ErrorCode = 2212221600;
                    response.Success = false;
                    return response;
                }

                user.Enabled = true;
                user.ConfirmationId = null;

                await unitOfWork.Session.UpdateAsync(user);
                return response;
            }, "Errore durante la conferma della email dell'utente");
        }

        public async Task<IRoleModel> CreateRoleIfNotExistsAsync(ICreateRoleDto dto)
        {
            IRoleModel? existingModel = await unitOfWork.Session.Query<IRoleModel>().SingleOrDefaultAsync(p => p.Code == dto.Code);
            if (existingModel != null)
            {
                mapper.Map(dto, existingModel);
                existingModel.Enabled = true;
                await unitOfWork.Session.UpdateAsync(existingModel);
            }
            else
            {
                existingModel = mapper.Map<IRoleModel>(dto);
                existingModel.Enabled = true;
                await unitOfWork.Session.SaveAsync(existingModel);
            }

            return existingModel;
        }

        public async Task<IUserModel> CreateUserIfNotExistsAsync(ICreateUserDto dto, bool isSystemUser = false)
        {
            IUserModel? existingModel = await unitOfWork.Session.Query<IUserModel>().SingleOrDefaultAsync(p => p.UserName == dto.UserName);
            if (existingModel != null)
            {
                mapper.Map(dto, existingModel);
                await unitOfWork.Session.UpdateAsync(existingModel);
            }
            else
            {
                existingModel = mapper.Map<IUserModel>(dto);
                await unitOfWork.Session.SaveAsync(existingModel);
            }

            await AddRolesToUser(dto, existingModel);

            existingModel.PasswordHash = HashPassword(dto.Password);

            existingModel.IsSystemUser = isSystemUser;

            return existingModel;
        }

        public async Task<IApiResponse> InitServiceAsync()
        {
            try
            {
                unitOfWork.BeginTransaction();
                var administratorsRole = await CreateRoleIfNotExistsAsync(new CreateRoleDto(RolesCodes.ADMINISTRATOR, "Administrators"));
                var usersRole = await CreateRoleIfNotExistsAsync(new CreateRoleDto(RolesCodes.USERS, "Users"));

                await CreateUserIfNotExistsAsync(new CreateUserDto("admin", "Pa$$w0rd01!", new string[] { administratorsRole.Id.ToString() }) { Email = "admin@test.com" });
                await CreateUserIfNotExistsAsync(new CreateUserDto("user", "Pa$$w0rd01!", new string[] { usersRole.Id.ToString() }) { Email = "user@test.com" });

                // Create System User
                await CreateUserIfNotExistsAsync(new CreateUserDto("system", "Pa$$w0rd01!", new string[] { administratorsRole.Id.ToString() }) { Email = "system@test.com" }, true);


                await unitOfWork.TryCommitOrRollbackAsync();
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.GetBaseException().Message);
            }
            return new ApiResponse();
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

        public virtual async Task<IApiResponse<IRefreshTokenViewModel>> RefreshAccessTokenAsync(IRefreshTokenRequestDto refreshTokenRequest, string? clientIp = null)
        {
            return await ExecuteTransactionAsync<IRefreshTokenViewModel>(async (response) =>
            {
                if (string.IsNullOrWhiteSpace(refreshTokenRequest?.AccessToken))
                {
                    response.ErrorMessage = T("Il campo AccessToken è obbligatorio!");
                    response.ErrorCode = 2301100845;
                    response.Success = false;
                    return response;
                }

                var principal = tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Il refresh token non è valido.");
                    response.ErrorCode = 202020000;
                    OnAuthEvent("N/D", false, T("Il refresh token non è valido."), clientIp);
                    return response;
                }

                var user = await authRepository.GetUserByIdAsync(userId.ToGuid());
                if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpire <= DateTime.UtcNow)
                {
                    response.Success = false;
                    response.ErrorMessage = T("Refresh token non valido o scaduto.");
                    response.ErrorCode = 202020001;
                    OnAuthEvent(user?.UserName ?? "N/D", false, T("Refresh token non valido o scaduto."), clientIp);
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
                OnAuthEvent(user.UserName, true, T("Nuovo refresh token generato."), clientIp);

                return response;
            }, "Errore durante il rinnovo del token");
        }

        public virtual async Task<IApiResponse<string>> RegisterNewUserAsync(IAuthRegisterDto authRegisterDto, string[]? permissionsCode = null, string[]? rolesCode = null)
        {
            return await ExecuteTransactionAsync<string>(async (response) =>
            {
                if (string.IsNullOrWhiteSpace(authRegisterDto?.UserName))
                {
                    response.ErrorMessage = T("Il nome utente è obbligatorio!");
                    response.ErrorCode = 2301100841;
                    response.Success = false;
                    return response;
                }

                if (string.IsNullOrWhiteSpace(authRegisterDto?.Password))
                {
                    response.ErrorMessage = T("La password è obbligatoria!");
                    response.ErrorCode = 2301100842;
                    response.Success = false;
                    return response;
                }

                if (string.IsNullOrWhiteSpace(authRegisterDto?.Email))
                {
                    response.ErrorMessage = T("L'email è obbligatoria!");
                    response.ErrorCode = 2301100843;
                    response.Success = false;
                    return response;
                }

                if (await unitOfWork.Session.Query<IUserModel>().AnyAsync(p => p.UserName == authRegisterDto.UserName))
                {
                    response.ErrorMessage = T("Il nome utente è già registrato");
                    response.ErrorCode = 2212211648;
                    response.Success = false;
                    return response;
                }

                if (await unitOfWork.Session.Query<IUserModel>().AnyAsync(p => p.Email == authRegisterDto.Email))
                {
                    response.ErrorMessage = T("L'indirizzo email è già registrato");
                    response.ErrorCode = 2212211649;
                    response.Success = false;
                    return response;
                }

                var model = mapper.Map<IUserModel>(authRegisterDto);

                if (!securityService.CheckPasswordValidity(authRegisterDto.Password, out string? passwordCheckingError))
                {
                    response.Success = false;
                    response.ErrorMessage = T("La password non soddisfa i criteri di sicurezza: '{0}'", passwordCheckingError ?? "N/D");
                    response.ErrorCode = 2212042300;
                    return response;
                }

                model.PasswordHash = HashPassword(authRegisterDto.Password);

                // If Email autentication is not active enable user now
                if (!securitySettings.VerifyEmail)
                {
                    model.Enabled = true;
                }

                await unitOfWork.Session.SaveAsync(model);

                if (permissionsCode != null && permissionsCode.Any() && model is IPermissionedUser pUser)
                {
                    foreach (var permissionCode in permissionsCode)
                    {
                        await securityService.AddPermissionToUserAsync(permissionCode, pUser, PermissionType.None);
                    }
                }
             
                await AddRolesToUser(rolesCode, model);

                await securityService.SendRegistrationConfirmAsync(model);

                response.Value = model.Id.ToString();
                return response;
            }, "Errore durante la registrazione dell'utente");
        }

        public async Task<IApiResponse> RequestRecoveryUserPasswordLinkAsync(IRequestRecoveryUserPasswordLinkDto recoveryUserPasswordDto)
        {
            return await ExecuteTransactionAsync(async (response) =>
            {
                var user = await unitOfWork.Session.Query<IUserModel>().SingleOrDefaultAsync(u => u.UserName == recoveryUserPasswordDto.UserName && u.Email == recoveryUserPasswordDto.Email);
                if (user is null)
                {
                    response.ErrorMessage = T("Impossibile trovare l'utente o l'email indicata");
                    response.ErrorCode = 2301091518;
                    response.Success = false;
                    return response;
                }

                await securityService.SendRecoveryPasswordLinkAsync(user);

                return response;
            }, "Errore durante il ripristino della password dell'utente");
        }

        protected void OnAuthEvent(string userName, bool success, string message, string? clientIp = null) =>
                                         AuthEvent?.Invoke(this, new AuthEventDto(userName, success, message, clientIp));

        protected void OnLoginEvent(string userName, bool success, string message, string? clientIp = null) =>
                                            LoginEvent?.Invoke(this, new AuthEventDto(userName, success, message, clientIp));

        private static bool CheckHashedPassword(IUserModel user, string clearPassword)
        {
            var passwordHash = user.PasswordHash ?? throw new BsException(2302060929, "The hash of the password cannot be null. Check the database record for the user.");
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

        public async Task AddRolesToUser(string[]? rolesCode, IUserModel? existingModel)
        {
            if (rolesCode != null && existingModel is IRoledUser roledUser)
            {
                roledUser.Roles ??= new List<IRoleModel>();

                foreach (var code in rolesCode)
                {
                    roledUser.Roles.AddIfNotExists(await unitOfWork.Session.Query<IRoleModel>().Where(r=>r.Code == code).FirstOrDefaultAsync(), r=>r.Code);
                }
            }
        }

        public async Task AddRolesToUser(ICreateUserDto dto, IUserModel? existingModel)
        {
            if (dto.RolesIds != null && existingModel is IRoledUser roledUser)
            {
                roledUser.Roles ??= new List<IRoleModel>();

                foreach (var roleId in dto.RolesIds)
                {
                    roledUser.Roles.AddIfNotExists(await unitOfWork.Session.GetAsync<IRoleModel>(roleId.ToGuid()), r => r.Id);
                }
            }
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

        private string HashPassword(string? clearPassword)
        {
            if (string.IsNullOrWhiteSpace(clearPassword))
            {
                throw new BsException(2212221238, T("Impossibile mascherare una password vuota!"));
            }
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