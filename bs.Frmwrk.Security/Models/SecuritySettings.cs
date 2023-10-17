using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Security.Models
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Models.Configuration.ISecuritySettings" />
    public class SecuritySettings : ISecuritySettings
    {
        /// <summary>
        /// Gets or sets the valid token issuer.
        /// </summary>
        /// <value>
        /// The valid token issuer.
        /// </value>
        public string? ValidTokenIssuer { get; set; }

        /// <summary>
        /// Gets or sets the valid token audience.
        /// </summary>
        /// <value>
        /// The valid token audience.
        /// </value>
        public string? ValidTokenAudience { get; set; }

        /// <summary>
        /// Gets or sets the validate issuer.
        /// </summary>
        /// <value>
        /// The validate issuer.
        /// </value>
        public bool? ValidateIssuer { get; set; }

        /// <summary>
        /// Gets or sets the validate audience.
        /// </summary>
        /// <value>
        /// The validate audience.
        /// </value>
        public bool? ValidateAudience { get; set; }

        /// <summary>
        /// Gets or sets the JWT refresh token validity minutes.
        /// </summary>
        /// <value>
        /// The JWT refresh token validity minutes.
        /// </value>
        public int? JwtRefreshTokenValidityMinutes { get; set; }

        /// <summary>
        /// Gets or sets the JWT token validity minutes.
        /// </summary>
        /// <value>
        /// The JWT token validity minutes.
        /// </value>
        public int? JwtTokenValidityMinutes { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public string? Secret { get; set; }

        /// <summary>
        /// Gets or sets the interval of failed access to count for check security violation ad disable user/ip.
        /// </summary>
        /// <value>
        /// The interval of failed access to count for check security violation ad disable user/ip.
        /// </value>
        public int? FailedAccessMonitoringPeriodInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the failed access maximum attempts.
        /// </summary>
        /// <value>
        /// The failed access maximum attempts.
        /// </value>
        public int? FailedAccessMaxAttempts { get; set; }

        /// <summary>
        /// Gets or sets the validate token issuer.
        /// </summary>
        /// <value>
        /// The validate token issuer.
        /// </value>
        public bool? ValidateTokenIssuer { get; set; }

        /// <summary>
        /// Gets or sets the validate token audience.
        /// </summary>
        /// <value>
        /// The validate token audience.
        /// </value>
        public bool? ValidateTokenAudience { get; set; }

        /// <summary>
        /// Gets or sets the password complexity.
        /// </summary>
        /// <value>
        /// The password complexity.
        /// </value>
        public PasswordScore PasswordComplexity { get; set; } = PasswordScore.Weak;

        /// <summary>
        /// Gets a value indicating whether [verify email].
        /// </summary>
        /// <value>
        /// <c>true</c> if [verify email]; otherwise, <c>false</c>.
        /// </value>
        public bool VerifyEmail { get; set; } = false;

        /// <summary>
        /// Gets or sets the Google recaptcha API secret (https://www.google.com/recaptcha/about/).
        /// </summary>
        /// <value>
        /// The recaptcha API secret.
        /// </value>
        public string RecaptchaApiSecret { get; set; }

        /// <summary>
        /// Gets or sets the recaptcha API endpoint (https://www.google.com/recaptcha/about/).
        /// </summary>
        /// <value>
        /// The recaptcha API endpoint.
        /// </value>
        public string RecaptchaApiEndpoint { get; set; } = "https://www.google.com/recaptcha/api/siteverify";

        /// <summary>
        /// Gets or sets the recaptcha minimum score (https://www.google.com/recaptcha/about/).
        /// </summary>
        /// <value>
        /// The recaptcha minimum score.
        /// </value>
        public decimal RecaptchaMinimumScore { get; set; } = 0.4m;

        /// <summary>
        /// Gets or sets a value indicating whether [recaptcha is enabled] on login and register.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [recaptcha enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool RecaptchaEnabled { get; set; } = false;
    }
}