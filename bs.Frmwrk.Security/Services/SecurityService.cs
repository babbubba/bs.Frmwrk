using bs.Data.Interfaces;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Globals.Security;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mailing;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Common;
using bs.Frmwrk.Mailing.Dtos;
using bs.Frmwrk.Security.Dtos;
using bs.Frmwrk.Security.Utilities;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHibernate.Linq;
using System.Data;

namespace bs.Frmwrk.Security.Services
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Services.Security.ISecurityService" />
    public class SecurityService : ISecurityService
    {
        private readonly ICoreSettings coreSettings;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<SecurityService> logger;
        private readonly IMailingService mailingService;
        private readonly IMapperService mapper;
        private readonly ISecuritySettings securitySettings;
        private readonly ITranslateService translateService;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="securitySettings">The security settings.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="securityRepository">The security repository.</param>
        /// <param name="translateService">The translate service.</param>
        public SecurityService(ILogger<SecurityService> logger, ISecuritySettings securitySettings, IUnitOfWork unitOfWork, ITranslateService translateService, IMapperService mapper, ICoreSettings coreSettings, IMailingService mailingService, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.securitySettings = securitySettings;
            this.unitOfWork = unitOfWork;
            this.translateService = translateService;
            this.mapper = mapper;
            this.coreSettings = coreSettings;
            this.mailingService = mailingService;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Occurs when [security event].
        /// </summary>
        public event EventHandler<ISecurityEventDto>? SecurityEvent;

        /// <summary>
        /// Occurs when [too many attempts event].
        /// </summary>
        public event EventHandler<ISecurityEventDto>? TooManyAttemptsEvent;

        public virtual async Task AddPermissionsToUserAsync(string[]? permissionsCode, IUserModel user, PermissionType? permissionType)
        {
            if (permissionsCode != null && user is IPermissionedUser permissionedUser)
            {
                permissionedUser.UsersPermissions ??= new List<IUsersPermissionsModel>();

                foreach (var code in permissionsCode)
                {
                    await AddPermissionToUserAsync(code, user, permissionType);
                }
            }
        }

        public virtual async Task AddPermissionToRoleAsync(string permissionCode, IRoleModel role, PermissionType? permissionType)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role), translateService.Translate("Il ruolo non può essere null"));
            }

            if (role is IPermissionedRole permissionedRole)
            {
                IRolesPermissionsModel rolePermission = permissionedRole.RolesPermissions?.FirstOrDefault(up => up?.Permission?.Code == permissionCode) ?? typeof(IRolesPermissionsModel).GetImplInstanceFromInterface<IRolesPermissionsModel>();
                rolePermission.Permission ??= await unitOfWork.Session.Query<IPermissionModel>().Where(x => x.Code == permissionCode).FirstOrDefaultAsync() ?? throw new Exception(translateService.Translate("Impossibile trovare il permesso con codice {0}", permissionCode));
                rolePermission.Role ??= (IRoleModel)permissionedRole ?? throw new Exception(translateService.Translate("Impossibile trovare il ruolo corrente"));
                rolePermission.Type = permissionType ?? PermissionType.None;
                await unitOfWork.Session.SaveOrUpdateAsync(rolePermission);
                permissionedRole.RolesPermissions.AddIfNotExists(rolePermission, x => x.Permission.Code);
            }
            else
            {
                logger.LogWarning(translateService.Translate("Il ruolo '{0}' non implementa la gestione dei permessi", role.Code));
            }
        }

        /// <summary>
        /// Adds the permission to user asynchronous.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="user">The user.</param>
        /// <param name="permissionType">Type of the permission (default is None).</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.Exception"></exception>
        public virtual async Task AddPermissionToUserAsync(string permissionCode, IUserModel user, PermissionType? permissionType)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), translateService.Translate("L'utente non può essere null"));
            }

            if (user is IPermissionedUser permissionedUser)
            {
                IUsersPermissionsModel userPermission = permissionedUser.UsersPermissions?.FirstOrDefault(up => up?.Permission?.Code == permissionCode) ?? typeof(IUsersPermissionsModel).GetImplInstanceFromInterface<IUsersPermissionsModel>();
                userPermission.Permission ??= await unitOfWork.Session.Query<IPermissionModel>().Where(x => x.Code == permissionCode).FirstOrDefaultAsync() ?? throw new Exception(translateService.Translate("Impossibile trovare il permesso con codice {0}", permissionCode));
                userPermission.User ??= (IUserModel)permissionedUser ?? throw new Exception(translateService.Translate("Impossibile trovare l'utente corrente"));
                userPermission.Type = permissionType ?? PermissionType.None;
                await unitOfWork.Session.SaveOrUpdateAsync(userPermission);
                permissionedUser.UsersPermissions.AddIfNotExists(userPermission, x => x.Permission.Code);
            }
            else
            {
                logger.LogWarning(translateService.Translate("L'utente '{0}' non implementa la gestione dei permessi", user.UserName));
            }
        }

        public virtual async Task AddRolesToUser(string[]? rolesCode, IUserModel? user)
        {
            if (rolesCode != null && user is IRoledUser roledUser)
            {
                roledUser.Roles ??= new List<IRoleModel>();

                foreach (var code in rolesCode)
                {
                    if (roledUser.Roles.AddIfNotExists(await unitOfWork.Session.Query<IRoleModel>().Where(r => r.Code == code).FirstOrDefaultAsync(), r => r.Id))
                    {
                        logger.LogDebug(translateService.Translate("Added role code '{0}' to user '{1}'", code, user.UserName));
                    }
                }
            }
        }

        public virtual async Task AddRolesToUser(Guid[]? rolesId, IUserModel? user)
        {
            if (rolesId != null && user is IRoledUser roledUser)
            {
                roledUser.Roles ??= new List<IRoleModel>();

                foreach (var roleId in rolesId)
                {
                    if (roledUser.Roles.AddIfNotExists(await unitOfWork.Session.GetAsync<IRoleModel>(roleId), r => r.Id))
                    {
                        logger.LogDebug(translateService.Translate("Added role id '{0}' to user '{1}'", roleId, user.UserName));
                    }
                }
            }
        }

        /// <summary>
        /// Checks the google recaptcha token v3 in Google Recaptcha API (only if Security:RecaptchaEnabled is setted to true in config file).
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">
        /// 2310171205
        /// or
        /// 2310171203 - Google Recaptcha Api validation response deserialization returns null value
        /// or
        /// 2310171204
        /// or
        /// 2310171206
        /// or
        /// 2310171207
        /// or
        /// 2310171208
        /// </exception>
        public virtual async Task<bool> CheckGoogleRecaptchaAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                var errorMessage = "Invalid token from Google Recaptcha Validation request";
                logger.LogWarning(errorMessage);
                throw new BsException(2310171205, errorMessage);
            }

            if (string.IsNullOrWhiteSpace(securitySettings.RecaptchaApiSecret))
            {
                var errorMessage = "Invalid Secret Key in Google Recaptcha Validation request. Set the configuration property Security:RecaptchaApiSecret with value provided in Google Recaptcha admin console";
                logger.LogWarning(errorMessage);
                throw new BsException(2310171206, errorMessage);
            }

            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("POST");
                request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var urlToCall = securitySettings.RecaptchaApiEndpoint + $"?secret={securitySettings.RecaptchaApiSecret}" + $"&response={token}";
                var endpointUri = new Uri(urlToCall);
                request.RequestUri = endpointUri;

                var response = await httpClientFactory.CreateClient(Recaptcha.HTTP_CLIENT_FACTORY_NAME).SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                var status = (int)response.StatusCode;

                if (response.Content != null && response.Content.Headers != null && (status == 200 || status == 201))
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        // valid response content
                        IRecaptchaResponseDto? responseBody;
                        try
                        {
                            responseBody = JsonConvert.DeserializeObject<RecaptchaResponseDto>(responseContent);
                            if (responseBody == null)
                            {
                                throw new BsException(2310171203, "Google Recaptcha Api validation response deserialization returns null value");
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = "Cannot deserialize Google Recaptcha Api validation response";
                            logger.LogWarning(ex, errorMessage);
                            throw new BsException(2310171204, errorMessage, ex);
                        }
                        if (!responseBody.Success || responseBody.Score < securitySettings.RecaptchaMinimumScore) return false;
                        return true;
                    }
                    else
                    {
                        const string errorMessage = "Call to Google API return empty body";
                        logger.LogWarning(errorMessage);
                        throw new BsException(2310171206, errorMessage);
                    }
                }
                else if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    string errorMessage = $"Google API returned the status code: {status}, details: {responseContent}";
                    logger.LogError(errorMessage);
                    throw new BsException(2310171207, errorMessage);
                }
                else
                {
                    string errorMessage = $"Google API returned the status code: {status}, details: 'N/D'";
                    logger.LogError(errorMessage);
                    throw new BsException(2310171208, errorMessage);
                }
            }
        }

        /// <summary>
        /// Checks the password validity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public bool CheckPasswordValidity(string? password, out string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = translateService.Translate("La password è vuota");
                return false;
            }

            var currentPasswordScore = PasswordAdvisor.CheckStrength(password);
            if (currentPasswordScore <= securitySettings.PasswordComplexity)
            {
                var errordetail = PasswordAdvisor.GetPasswordScoreTips(securitySettings.PasswordComplexity);
                errorMessage = translateService.Translate("La password non è sufficientemente complessa, la complessità della password è '{1}' ma è richiesto un livello superiore a '{0}'. {2}", securitySettings.PasswordComplexity.ToString(), currentPasswordScore.ToString(), errordetail);
                return false;
            }

            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Checks the user permission asynchronous (if user has role admin it always return true).
        /// If the user role implement IPermissionedRole this will check if requested permission code is valid for the role's permissions.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public async Task<bool> CheckUserPermissionAsync(IUserModel? user, string permissionCode, PermissionType type = PermissionType.None)
        {
            bool result = false;
            bool verified = false;

            if (user is null)
            {
                return result;
            }

            // Administrator are allowed always
            if (user is IRoledUser roledUser)
            {
                // Skip check if user is admin
                if (user.IsAdmin())
                {
                    result = true;
                    verified = true;
                }
                else
                {
                    //enumerate user's roles permissions
                    result = roledUser.Roles.Cast<IPermissionedRole>().SelectMany(r => r.RolesPermissions).Any(up => up.Permission.Code == permissionCode && type <= up.Type);
                    verified = true;
                }
            }

            // Check users' permission and type
            if (user is IPermissionedUser permissionedUser && !result)
            {
                result = permissionedUser.UsersPermissions.Any(up => up.Permission.Code == permissionCode && type <= up.Type);
                verified = true;
            }

            if (!verified && user is not IPermissionedUser)
            {
                // Skip check
                result = true;
                OnSecurityEvent(translateService.Translate("Saltata verifica del permesso (codice: {1}) per l' utente {0} perche non implementa i permessi", result ? "riuscita" : "fallita", permissionCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, user?.UserName ?? "N/D");
            }

            OnSecurityEvent(translateService.Translate("Verifica del permesso (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", permissionCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, user?.UserName ?? "N/D");
            return result;
        }

        /// <summary>
        /// Checks the user role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleCode">The role code.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2212081134
        /// or
        /// 2212081135</exception>
        public async Task<bool> CheckUserRoleAsync(IRoledUser? user, string roleCode)
        {
            if (user == null)
            {
                throw new BsException(2212081134, translateService.Translate("Utente non valido"));
            }

            if (roleCode == null)
            {
                throw new BsException(2212081135, translateService.Translate("Ruolo non valido"));
            }

            var result = user.Roles?.Any(r => r.Code == roleCode) ?? false;
            OnSecurityEvent(translateService.Translate("Verifica del ruolo (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", roleCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, (user is IUserModel u) ? u.UserName : "N/D");
            return result;
        }

        /// <summary>
        /// Checks if the user is memebership of one of the roles specified asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="rolesCode"></param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2310051105 if user is not implementing roles</exception>
        public async Task<bool> CheckUserRolesAsync(IUserModel user, string[] rolesCode)
        {
            if (user is IRoledUser roledUser)
            {
                var result = new List<bool>();
                foreach (var roleCode in rolesCode)
                {
                    result.Add(await CheckUserRoleAsync(roledUser, roleCode));
                }
                return result.Exists(r => r);
            }
            else
            {
                throw new BsException(2310051105, translateService.Translate("L'utente non implementa la gestione dei ruoli"));
            }
        }

        public async Task<IPermissionModel> CreatePermissionIfNotExistsAsync(ICreatePermissionDto dto)
        {
            IPermissionModel? existingPermission = await unitOfWork.Session.Query<IPermissionModel>().SingleOrDefaultAsync(p => p.Code == dto.Code);
            if (existingPermission != null)
            {
                mapper.Map(dto, existingPermission);
                await unitOfWork.Session.UpdateAsync(existingPermission);
            }
            else
            {
                existingPermission = mapper.Map<IPermissionModel>(dto);
                await unitOfWork.Session.SaveAsync(existingPermission);
            }

            return existingPermission;
        }

        public string GetConfirmRegistrationUrl(string userId, string confirmationId)
        {
            return $"{coreSettings.FrontendConfirmEmailUrl?.TrimEnd('/').TrimEnd('\\')}?UserId={userId}&ConfirmationId={confirmationId}";
        }

        public IApiResponse<ISelectListItem> GetPasswordScore(string password)
        {
            PasswordScore score = 0;
            if (!string.IsNullOrWhiteSpace(password))
            {
                score = PasswordAdvisor.CheckStrength(password);
            }

            return new ApiResponse<ISelectListItem>(new SelectListItem(((int)score).ToString(), score.ToString()));
        }

        public string GetRecoveryPasswordUrl(string userId, string recoveryPasswordId)
        {
            return $"{coreSettings.FrontendRecoveryPasswordUrl?.TrimEnd('/').TrimEnd('\\')}?UserId={userId}&RecoveryPasswordId={recoveryPasswordId}";
        }

        public async Task<IApiResponse> InitServiceAsync()
        {
            // Create the default permission
            try
            {
                unitOfWork.BeginTransaction();
                await CreatePermissionIfNotExistsAsync(new CreatePermissionDto(PermissionsCodes.USERS_REGISTRY, "Anagrafica utenti"));
                await CreatePermissionIfNotExistsAsync(new CreatePermissionDto(PermissionsCodes.ROLES_REGISTRY, "Anagrafica ruoli"));
                await CreatePermissionIfNotExistsAsync(new CreatePermissionDto(PermissionsCodes.PERMISSIONS_REGISTRY, "Anagrafica permessi"));
                await CreatePermissionIfNotExistsAsync(new CreatePermissionDto(PermissionsCodes.USERS_MODERATION, "Moderazione utenti"));

                await unitOfWork.TryCommitOrRollbackAsync();
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.GetBaseException().Message);
            }
            return new ApiResponse();
        }

        public async Task SendRecoveryPasswordLinkAsync(IUserModel user)
        {
            if (user.Enabled)
            {
                //send email link
                user.RecoveryPasswordId = Guid.NewGuid();
                var message = new MailMessageDto();
                message.Subject = translateService.Translate($"Ripristino password");
                message.IsHtmlBody = true;
                message.ToEmails = new string[] { user.Email };
                message.Body = @$"<p>Clicca <a href=""{GetRecoveryPasswordUrl(user.Id.ToString(), user.RecoveryPasswordId.ToString())}""> qui </a> per ripristinare la password.</p></br><p>Se il link non funziona copia ed incolla nel browser il seguente url: {GetRecoveryPasswordUrl(user.Id.ToString(), user.RecoveryPasswordId.ToString())}</p>";
                await mailingService.SendEmailAsync(message);
            }
            else
            {
                logger.LogWarning("User try resetting password but was not enabled");
            }
        }

        public async Task SendRegistrationConfirmAsync(IUserModel user)
        {
            if (securitySettings.VerifyEmail)
            {
                //send email link
                user.ConfirmationId = Guid.NewGuid();
                var message = new MailMessageDto
                {
                    Subject = translateService.Translate($"Conferma email"),
                    IsHtmlBody = true,
                    ToEmails = new string[] { user.Email },
                    Body = @$"<p>Clicca <a href=""{GetConfirmRegistrationUrl(user.Id.ToString(), user.ConfirmationId.ToString())}""> qui </a> per confermare il tuo indirizzo email.</p></br><p>Se il link non funziona copia ed incolla nel browser il seguente url: {GetConfirmRegistrationUrl(user.Id.ToString(), user.ConfirmationId.ToString())}</p>"
                };
                await mailingService.SendEmailAsync(message);
            }
        }

        /// <summary>
        /// Tracks the login fail asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="clientIp">The client ip.</param>
        public virtual async Task TrackLoginFailAsync(string username, string? clientIp)
        {
            var modelType = typeof(IAuditFailedLoginModel).GetImplTypeFromInterface() ?? throw new BsException(2212191126, translateService.Translate("Impossibile trovare una implementazione del modello 'IAuditFailedLoginModel'"));

            IAuditFailedLoginModel newEntry = (IAuditFailedLoginModel)Activator.CreateInstance(modelType)!;
            newEntry.ClientIp = clientIp;
            newEntry.EventDate = DateTime.UtcNow;
            newEntry.UserName = username;

            await unitOfWork.Session.SaveAsync(newEntry);

            DateTime periodToCheckBegin = DateTime.UtcNow.AddMinutes(-securitySettings.FailedAccessMonitoringPeriodInMinutes ?? -10);

            var usernameAttemptsInLastPeriod = await unitOfWork.Session.Query<IAuditFailedLoginModel>().Where(a => a.EventDate > periodToCheckBegin && a.UserName.ToLower() == username.ToLower()).CountAsync();

            if (usernameAttemptsInLastPeriod > (securitySettings.FailedAccessMaxAttempts ?? 5))
            {
                //var usernameToDisable = await securityRepository.GetUserByUserNameAsync(username);
                var usernameToDisable = await unitOfWork.Session.Query<IUserModel>().FirstOrDefaultAsync(u => u.UserName == username);
                if (usernameToDisable is not null)
                {
                    usernameToDisable.Enabled = false;
                    OnTooManyAttemptsEvent(translateService.Translate("Troppi tentativi di accesso falliti per l'utente", username), SecurityEventSeverity.Danger, username, clientIp ?? "*");
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l'utente: '{0}', l'utente è stato disabilitato", username.ToLower()));
                }
            }

            if (clientIp is not null)
            {
                var ipAttemptsInLastPeriod = await unitOfWork.Session.Query<IAuditFailedLoginModel>().Where(a => a.EventDate > periodToCheckBegin && a.ClientIp != null && a.ClientIp.ToLower() == clientIp.ToLower()).CountAsync();
                if (ipAttemptsInLastPeriod > (securitySettings.FailedAccessMaxAttempts ?? 5))
                {
                    OnTooManyAttemptsEvent(translateService.Translate("Troppi tentativi di accesso falliti dall'ip '{0}", clientIp), SecurityEventSeverity.Danger, username, clientIp);
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l' ip '{0}'", clientIp?.ToLower() ?? "*"));
                }
            }
        }

        /// <summary>
        /// Called when [security event].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="clientIp">The client ip.</param>
        protected void OnSecurityEvent(string message, SecurityEventSeverity severity, string userName, string? clientIp = null) =>
                                                    SecurityEvent?.Invoke(this, new SecurityEventDto(message, severity, userName, clientIp));

        /// <summary>
        /// Called when [too many attempts event event].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="clientIp">The client ip.</param>
        protected void OnTooManyAttemptsEvent(string message, SecurityEventSeverity severity, string userName, string? clientIp = null) =>
                                                    TooManyAttemptsEvent?.Invoke(this, new SecurityEventDto(message, severity, userName, clientIp));
    }
}