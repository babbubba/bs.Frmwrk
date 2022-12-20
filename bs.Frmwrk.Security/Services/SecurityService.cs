﻿using bs.Data.Interfaces;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Common;
using bs.Frmwrk.Security.Dtos;
using bs.Frmwrk.Security.Models;
using bs.Frmwrk.Security.Utilities;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

#pragma warning disable CS1998

namespace bs.Frmwrk.Security.Services
{

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Services.Security.ISecurityService" />
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SecurityService> logger;

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
        private readonly IMapperService mapper;

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
        public SecurityService(ILogger<SecurityService> logger, ISecuritySettings securitySettings, IUnitOfWork unitOfWork, ISecurityRepository securityRepository, ITranslateService translateService, IMapperService mapper)
        {
            this.logger = logger;
            this.securitySettings = securitySettings;
            this.unitOfWork = unitOfWork;
            this.securityRepository = securityRepository;
            this.translateService = translateService;
            this.mapper = mapper;
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
        /// Checks the password validity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public bool CheckPasswordValidity(string password, out string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = translateService.Translate("La password è vuota.");
                return false;
            }

            var currentPasswordScore = PasswordAdvisor.CheckStrength(password);
            if (currentPasswordScore <= securitySettings.PasswordComplexity)
            {
                errorMessage = translateService.Translate("La password non è sufficientemente complessita (la complessità della password è '{1}' ma è richiesto '{0}').", securitySettings.PasswordComplexity.ToString(), currentPasswordScore.ToString());
                return false;
            }

            errorMessage = null;
            return true;
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

        /// <summary>
        /// Checks the user permission asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> CheckUserPermissionAsync(IPermissionedUser user, string permissionCode, PermissionType type = PermissionType.None)
        {
            bool result = false;

            // Administrator are allowed always
            if (user is IRoledUser roledUser)
            {
                if (await CheckUserRoleAsync(roledUser, DefaultRolesCodes.Administrators)) return true;
            }

            // Check users' permission and type
            if (user is IPermissionedUser permissionedUser)
            {
                result = permissionedUser.UsersPermissions.Any(up => up.Permission.Code == permissionCode && type <= up.Type);
            }
            else
            {
                //throw new BsException(2212191003, translateService.Translate("Utente non implementa la gestione dei permessi."));
                // Skip check
                result = false;
            }

            OnSecurityEvent(translateService.Translate("Verifica del permesso (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", permissionCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, (user is IUserModel u) ? u.UserName : "N/D");
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
                throw new BsException(2212081134, translateService.Translate("Utente non valido controllando il ruolo."));
            }

            if (roleCode == null)
            {
                throw new BsException(2212081135, translateService.Translate("Ruolo non valido controllando il ruolo."));
            }
            var result = user.Roles.Any(r => r.Code == roleCode);
            OnSecurityEvent(translateService.Translate("Verifica del ruolo (codice: {1}) per l' utente {0}", result ? "riuscita" : "fallita", roleCode), result ? SecurityEventSeverity.Verbose : SecurityEventSeverity.Warning, (user is IUserModel u) ? u.UserName : "N/D");
            return result;
        }

        /// <summary>
        /// Tracks the login fail asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="clientIp">The client ip.</param>
        public virtual async Task TrackLoginFailAsync(string username, string? clientIp)
        {
            var modelType = typeof(IAuditFailedLoginModel).GetTypeFromInterface() ?? throw new BsException(2212191126, translateService.Translate("Impossibile trovare una implementazione del modello 'IAuditFailedLoginModel'"));

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
                    OnTooManyAttemptsEventEvent(translateService.Translate("Troppi tentativi di accesso falliti per l'utente", username), SecurityEventSeverity.Danger, username, clientIp??"*");
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l'utente: '{0}'. L'utente è stato disabilitato!", username.ToLower()));
                }
            }

            if(clientIp is not null)
            {
                var ipAttemptsInLastPeriod = await unitOfWork.Session.Query<IAuditFailedLoginModel>().Where(a => a.EventDate > periodToCheckBegin && a.ClientIp != null && a.ClientIp.ToLower() == clientIp.ToLower()).CountAsync();
                if (ipAttemptsInLastPeriod > (securitySettings.FailedAccessMaxAttempts ?? 5))
                {
                    OnTooManyAttemptsEventEvent(translateService.Translate("Troppi tentativi di accesso falliti dall'ip", username), SecurityEventSeverity.Danger, username, clientIp);
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l' ip '{0}'.", clientIp?.ToLower() ?? "*"));
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
        protected void OnTooManyAttemptsEventEvent(string message, SecurityEventSeverity severity, string userName, string? clientIp = null) =>
                                                    TooManyAttemptsEvent?.Invoke(this, new SecurityEventDto(message, severity, userName, clientIp));

        public async Task<IApiResponse> InitServiceAsync()
        {
            // Create the default permission
            try
            {
                unitOfWork.BeginTransaction();
                await CreatePermissionAsync(CreatePermissionDto("USERS_REGISTRY", "Anagrafica utenti"));
                await CreatePermissionAsync(CreatePermissionDto("ROLES_REGISTRY", "Anagrafica ruoli"));
                await CreatePermissionAsync(CreatePermissionDto("PERMISSIONS_REGISTRY", "Anagrafica permessi"));
                await unitOfWork.TryCommitOrRollbackAsync();
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.GetBaseException().Message);
            }
            return new ApiResponse();
        }

        public async Task<IPermissionModel> CreatePermissionAsync(IPermissionDto dto)
        {
            var model = mapper.Map<IPermissionModel>(dto);  
            await securityRepository.CreatePermissionAsync(model);
            return model;
        }

        public IPermissionDto CreatePermissionDto(string code, string name, bool enabled = true)
        {
            var permissionDtoType = typeof(IPermissionDto).GetTypeFromInterface() ?? throw new BsException(2212201042, translateService.Translate("Impossibile trovare una implementazione del modello 'IPermissionDto'"));
            IPermissionDto permissionEntry = (IPermissionDto)Activator.CreateInstance(permissionDtoType)!;
            permissionEntry.Code = code;
            permissionEntry.Name = name;
            permissionEntry.Enabled = enabled;
            return permissionEntry;

        }
    }

}

#pragma warning restore CS1998
