using bs.Data.Interfaces;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Security.Models;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;

namespace bs.Frmwrk.Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ILogger<SecurityService> logger;
        private readonly ISecuritySettings securitySettings;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISecurityRepository securityRepository;
        private readonly ITranslateService translateService;

        public SecurityService(ILogger<SecurityService> logger, ISecuritySettings securitySettings, IUnitOfWork unitOfWork, ISecurityRepository securityRepository, ITranslateService translateService)
        {
            this.logger = logger;
            this.securitySettings = securitySettings;
            this.unitOfWork = unitOfWork;
            this.securityRepository = securityRepository;
            this.translateService = translateService;
        }

        public bool CheckPasswordValidity(string password, out string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = translateService.Translate("La password è vuota.");
                return false;
            }

            if (password.Length < (securitySettings.PasswordMinLength ?? 1))
            {
                errorMessage = translateService.Translate("La password è troppo corta (caratteri necessari {0}).", securitySettings.PasswordMinLength ?? 1);
                return false;
            }

            //TODO: Implementa il check della validità della password con l'utility PasswordAdvisor
            errorMessage = null;
            return true;
        }

        public virtual async Task TrackLoginFailAsync(string username, string? clientIp)
        {
            // TODO: Implementa notifiche per accessi falliti ed utenti bloccati
            var newEntry = securityRepository.GetInstanceOfAuditFailedLogModel();
            newEntry.ClientIp = clientIp;
            newEntry.EventDate = DateTime.UtcNow;
            newEntry.UserName = username;

            await unitOfWork.Session.SaveAsync(newEntry);

            DateTime periodToCheckBaegin = DateTime.UtcNow.AddMinutes(-securitySettings.FailedAccessMonitoringPeriodInMinutes ?? 10);

            var usernameAttemptsInLastPeriod = await unitOfWork.Session.Query<AuditFailedLoginBaseModel>().Where(a => a.EventDate > periodToCheckBaegin && a.UserName.ToLower() == username.ToLower()).CountAsync();

            if (usernameAttemptsInLastPeriod > (securitySettings.FailedAccessMaxAttempts ?? 5))
            {
                var usernameToDisable = await securityRepository.GetUserByUserNameAsync(username);
                if (usernameToDisable is not null)
                {
                    usernameToDisable.Enabled = false;
                    //TooManyUserLoginAttemptsFailed?.Invoke(this, username.ToLower());
                    logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l'utente: '{0}'. L'utente è stato disabilitato!", username.ToLower()));
                    //var adminUser = await usersRepository.GetUserByUsernameAsync("admin");

                    //await mailingService.SendEmailAsync(new MailMessageDto
                    //{
                    //    ToEmails = new string[] { adminUser.Email },
                    //    Body = $"L'utente '{username}' ha fallito l'accesso per '{usernameAttemptsInLastPeriod}' volte nell'intervallo di {lastPeriodInMinutes} minuti ed è stato disabilitato per sicurezza. Ultimo IP utilizzato {clientIp}.",
                    //    Subject = $"Avviso di sicurezza. L'utente '{username}' è stato disabilitato per problemi di sicurezza.",
                    //    Priority = Infrastructure.Dtos.Mailing.MessagePriority.Urgent
                    //});
                }
            }

            var ipAttemptsInLastPeriod = await unitOfWork.Session.Query<AuditFailedLoginBaseModel>().Where(a => a.EventDate > periodToCheckBaegin && a.ClientIp != null && a.ClientIp.ToLower() == clientIp.ToLower()).CountAsync();
            if (ipAttemptsInLastPeriod > (securitySettings.FailedAccessMaxAttempts ?? 5))
            {
                //TooManyClientIpLoginAttemptsFailed?.Invoke(this, clientIp.ToLower());
                logger.LogError(translateService.Translate("Troppi tentativi di accesso falliti per l' ip '{0}'.", clientIp.ToLower() ?? "*"));
                //var adminUser = await usersRepository.GetUserByUsernameAsync("admin");

                //await mailingService.SendEmailAsync(new MailMessageDto
                //{
                //    ToEmails = new string[] { adminUser.Email },
                //    Body = $"Si sono verificati '{ipAttemptsInLastPeriod}' falliti nell'intervallo di {lastPeriodInMinutes} minuti dall'ip {clientIp}.",
                //    Subject = $"Avviso di sicurezza. Troppi accessi falliti provenienti dall'ip: {clientIp}.",
                //    Priority = Infrastructure.Dtos.Mailing.MessagePriority.Urgent
                //});
            }
        }

        public async Task<bool> CheckUserPermissionAsync(IPermissionedUser user, string permissionCode, PermissionType type = PermissionType.None)
        {
            // Administrator are allowed always
            if (user is IRoledUser roledUser)
            {
                if (await CheckUserRoleAsync(roledUser, DefaultRolesCodes.Administrators)) return true;
            }
            //TODO: Implementa CheckUserPermissionAsync
            throw new NotImplementedException();
        }

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

            return user.Roles.Any(r => r.Code == roleCode);
        }

        public IAuditFailedLoginModel GetInstanceOfAuditFailedLogModel()
        {
            throw new NotImplementedException();
        }
    }
}