using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Security.Models
{
    public class SecuritySettings : ISecuritySettings
    {
        public string? ValidTokenIssuer { get; set; }
        public string? ValidTokenAudience { get; set; }
        public bool? ValidateIssuer { get; set; }
        public bool? ValidateAudience { get; set; }
        public int? JwtRefreshTokenValidityMinutes { get; set; }
        public int? JwtTokenValidityMinutes { get; set; }
        public string? Secret { get; set; }
        public int? FailedAccessMonitoringPeriodInMinutes { get; set; }
        public int? FailedAccessMaxAttempts { get; set; }
        public int? PasswordMinLength { get; set; }
        public bool? ValidateTokenIssuer { get; set; }
        public bool? ValidateTokenAudience { get; set; }
        public PasswordScore PasswordComplexity { get; set; } = PasswordScore.Weak;

    }
}