using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Core.Models.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecuritySettings
    {
        /// <summary>
        /// Gets or sets the valid token issuer.
        /// </summary>
        /// <value>
        /// The valid token issuer.
        /// </value>
        string? ValidTokenIssuer { get; set; }

        /// <summary>
        /// Gets or sets the valid token audience.
        /// </summary>
        /// <value>
        /// The valid token audience.
        /// </value>
        string? ValidTokenAudience { get; set; }

        /// <summary>
        /// Gets or sets the validate issuer.
        /// </summary>
        /// <value>
        /// The validate issuer.
        /// </value>
        bool? ValidateIssuer { get; set; }

        /// <summary>
        /// Gets or sets the validate audience.
        /// </summary>
        /// <value>
        /// The validate audience.
        /// </value>
        bool? ValidateAudience { get; set; }

        /// <summary>
        /// Gets or sets the JWT refresh token validity minutes.
        /// </summary>
        /// <value>
        /// The JWT refresh token validity minutes.
        /// </value>
        int? JwtRefreshTokenValidityMinutes { get; set; }

        /// <summary>
        /// Gets or sets the JWT token validity minutes.
        /// </summary>
        /// <value>
        /// The JWT token validity minutes.
        /// </value>
        int? JwtTokenValidityMinutes { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        string? Secret { get; set; }

        /// <summary>
        /// Gets or sets the interval of failed access to count for check security violation ad disable user/ip.
        /// </summary>
        /// <value>
        /// The interval of failed access to count for check security violation ad disable user/ip.
        /// </value>
        int? FailedAccessMonitoringPeriodInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the failed access maximum attempts.
        /// </summary>
        /// <value>
        /// The failed access maximum attempts.
        /// </value>
        int? FailedAccessMaxAttempts { get; set; }


        /// <summary>
        /// Gets or sets the validate token issuer.
        /// </summary>
        /// <value>
        /// The validate token issuer.
        /// </value>
        bool? ValidateTokenIssuer { get; set; }

        /// <summary>
        /// Gets or sets the validate token audience.
        /// </summary>
        /// <value>
        /// The validate token audience.
        /// </value>
        bool? ValidateTokenAudience { get; set; }
        /// <summary>
        /// Gets or sets the password complexity.
        /// </summary>
        /// <value>
        /// The password complexity.
        /// </value>
        PasswordScore PasswordComplexity { get; set; }
        /// <summary>
        /// Gets a value indicating whether [verify email].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [verify email]; otherwise, <c>false</c>.
        /// </value>
        bool VerifyEmail { get; }
    }
}