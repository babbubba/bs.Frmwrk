namespace bs.Frmwrk.Core.Models.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public interface ICoreSettings
    {
        /// <summary>
        /// Gets or sets the external DLL files search pattern.
        /// </summary>
        /// <value>
        /// The external DLL files search pattern.
        /// </value>
        string? ExternalDllFilesSearchPattern { get; set; }

        /// <summary>
        /// Gets or sets the external DLL files root path.
        /// </summary>
        /// <value>
        /// The external DLL files root path.
        /// </value>
        string? ExternalDllFilesRootPath { get; set; }

        /// <summary>
        /// Gets or sets the frontend origins.
        /// </summary>
        /// <value>
        /// The frontend origins.
        /// </value>
        string[]? FrontendOrigins { get; set; }

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        string? AppTitle { get; set; }

        /// <summary>
        /// Gets or sets the application company.
        /// </summary>
        /// <value>
        /// The application company.
        /// </value>
        string? AppCompany { get; set; }

        /// <summary>
        /// Gets or sets the company web site.
        /// </summary>
        /// <value>
        /// The company web site.
        /// </value>
        string? CompanyWebSite { get; set; }

        /// <summary>
        /// Gets or sets the application roles.
        /// </summary>
        /// <value>
        /// The application roles.
        /// </value>
        IDictionary<string, string>? AppRoles { get; set; }

        /// <summary>
        /// Gets or sets the signal r hubs.
        /// </summary>
        /// <value>
        /// The signal r hubs.
        /// </value>
        IDictionary<string, string>? SignalRHubs { get; set; }

        /// <summary>
        /// Gets or sets the publish URL.
        /// </summary>
        /// <value>
        /// The publish URL.
        /// </value>
        string? PublishUrl { get; set; }

        /// <summary>
        /// Gets or sets the frontend confirm email URL (used in the email sent after registration when 'Security:VerifyEmail' is true).
        /// </summary>
        /// <value>
        /// The frontend confirm email URL.
        /// </value>
        string? FrontendConfirmEmailUrl { get; set; }

        /// <summary>
        /// Gets or sets the frontend recovery password URL.
        /// </summary>
        /// <value>
        /// The frontend recovery password URL.
        /// </value>
        string? FrontendRecoveryPasswordUrl { get; set; }
    }
}