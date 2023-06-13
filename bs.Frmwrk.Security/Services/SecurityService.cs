using bs.Data.Interfaces;
using bs.Frmwrk.Auth.Dtos;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Dtos.Mailing;
using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Globals.Security;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mailing;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Common;
using bs.Frmwrk.Mailing.Dtos;
using bs.Frmwrk.Security.Dtos;
using bs.Frmwrk.Security.Models;
using bs.Frmwrk.Security.Utilities;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;


namespace bs.Frmwrk.Security.Services
{

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Services.Security.ISecurityService" />
    public class SecurityService : ISecurityService
    {
        private readonly ICoreSettings coreSettings;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SecurityService> logger;

        private readonly IMailingService mailingService;

        private readonly IMapperService mapper;

        /// <summary>
        /// The security repository
        /// </summary>
        private readonly ISecurityRepository securityRepository;

        /// <summary>
        /// The security settings
        /// </summary>
        private readonly ISecuritySettings securitySettings;

        /// <summary>
        /// The translate service
        /// </summary>
        private readonly ITranslateService translateService;
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="securitySettings">The security settings.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="securityRepository">The security repository.</param>
        /// <param name="translateService">The translate service.</param>
        public SecurityService(ILogger<SecurityService> logger, ISecuritySettings securitySettings, IUnitOfWork unitOfWork, ISecurityRepository securityRepository, ITranslateService translateService, IMapperService mapper, ICoreSettings coreSettings, IMailingService mailingService)
        {
            this.logger = logger;
            this.securitySettings = securitySettings;
            this.unitOfWork = unitOfWork;
            this.securityRepository = securityRepository;
            this.translateService = translateService;
            this.mapper = mapper;
            this.coreSettings = coreSettings;
            this.mailingService = mailingService;
        }

        /// <summary>
        /// Occurs when [security event].
        /// </summary>
        public event EventHandler<ISecurityEventDto>? SecurityEvent;

        /// <summary>
        /// Occurs when [too many attempts event].
        /// </summary>
        public event EventHandler<ISecurityEventDto>? TooManyAttemptsEvent;


        /// <summary>
        /// Adds the permission to user asynchronous.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="user">The user.</param>
        /// <param name="permissionType">Type of the permission (default is None).</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.Exception"></exception>
        public async Task AddPermissionToUserAsync(string permissionCode, IPermissionedUser user, PermissionType? permissionType)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), translateService.Translate("L'utente non può essere null"));
            }

            IUsersPermissionsModel userPermission = user.UsersPermissions?.FirstOrDefault(up=>up?.Permission?.Code == permissionCode) ?? typeof(IUsersPermissionsModel).GetImplInstanceFromInterface<IUsersPermissionsModel>();
            userPermission.Permission ??= await unitOfWork.Session.Query<IPermissionModel>().Where(x => x.Code == permissionCode).FirstOrDefaultAsync() ?? throw new Exception(translateService.Translate("Impossibile trovare il permesso con codice {0}", permissionCode));
            userPermission.User ??= (IUserModel)user ?? throw new Exception(translateService.Translate("Impossibile trovare l'utente corrente")); 
            userPermission.Type = permissionType ?? PermissionType.None;
            await unitOfWork.Session.SaveOrUpdateAsync(userPermission);
            user.UsersPermissions.AddIfNotExists(userPermission, x => x.Permission.Code);
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
        /// Checks the user permission asynchronous if user implements permissions.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> CheckUserPermissionAsync(IPermissionedUser? user, string permissionCode, PermissionType type = PermissionType.None)
        {
            if (user is null)
            {
                return false;
            }

            bool result = false;

            // Administrator are allowed always
            if (user is IRoledUser roledUser)
            {
                if (await CheckUserRoleAsync(roledUser, RolesCodes.ADMINISTRATOR)) return true;
            }

            // Check users' permission and type
            if (user is IPermissionedUser permissionedUser)
            {
                result = permissionedUser.UsersPermissions.Any(up => up.Permission.Code == permissionCode && type <= up.Type);
            }
            else
            {
                // Skip check
                result = true;
            }

            OnSecurityEvent(translateService.Translate("Verifica del permesso (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", permissionCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, (user is IUserModel u) ? u.UserName : "N/D");
            return result;
        }


        //public async Task<bool> CheckUserPermissionAsync(IPermissionedUser user, string[] requiredPermissionsCodes, PermissionType type = PermissionType.None)
        //{

        //}

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
                throw new BsException(2212081134, translateService.Translate("Utente non valido controllando il ruolo"));
            }

            if (roleCode == null)
            {
                throw new BsException(2212081135, translateService.Translate("Ruolo non valido controllando il ruolo"));
            }
            var result = user.Roles.Any(r => r.Code == roleCode);
            OnSecurityEvent(translateService.Translate("Verifica del ruolo (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", roleCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, (user is IUserModel u) ? u.UserName : "N/D");
            return result;
        }

        public async Task<IPermissionModel> CreatePermissionIfNotExistsAsync(ICreatePermissionDto dto)
        {
            IPermissionModel? existingPermission = await unitOfWork.Session.Query<IPermissionModel>().SingleOrDefaultAsync(p => p.Code == dto.Code);
            if (existingPermission != null)
            {
                mapper.Map(dto, existingPermission);
                await securityRepository.UpdatePermissionAsync(existingPermission);
            }
            else
            {
                existingPermission = mapper.Map<IPermissionModel>(dto);
                await securityRepository.CreatePermissionAsync(existingPermission);
            }

            return existingPermission;
        }

        public string GetConfirmRegistrationUrl(string userId, string confirmationId)
        {
            return $"{coreSettings.PublishUrl?.TrimEnd('/').TrimEnd('\\')}/api/Auth/ConfirmEmail?UserId={userId}&ConfirmationId={confirmationId}";
        }

        public IApiResponse<ISelectListItem> GetPasswordScore(string password)
        {
            PasswordScore score = 0;
            if (!string.IsNullOrWhiteSpace(password))
            {
                score = PasswordAdvisor.CheckStrength(password);
            }

            return  new ApiResponse<ISelectListItem>(new SelectListItem(((int)score).ToString(), score.ToString()));
        }
        public string GetRecoveryPasswordUrl(string userId, string recoveryPasswordId)
        {
            return $"{coreSettings.PublishUrl?.TrimEnd('/').TrimEnd('\\')}/api/Auth/RecoveryPassword?UserId={userId}&RecoveryPasswordId={recoveryPasswordId}";
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
                var usernameToDisable = await securityRepository.GetUserByUserNameAsync(username);
                if (usernameToDisable is not null)
                {
                    usernameToDisable.Enabled = false;
                    OnTooManyAttemptsEvent(translateService.Translate("Troppi tentativi di accesso falliti per l'utente", username), SecurityEventSeverity.Danger, username, clientIp??"*");
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l'utente: '{0}', l'utente è stato disabilitato", username.ToLower()));
                }
            }

            if(clientIp is not null)
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

