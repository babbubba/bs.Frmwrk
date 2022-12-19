namespace bs.Frmwrk.Core.Models.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public interface ILoggingSettings
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        string? Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the log file.
        /// </summary>
        /// <value>
        /// The name of the log file.
        /// </value>
        string? LogFileName { get; set; }

        /// <summary>
        /// Gets or sets the debug.
        /// </summary>
        /// <value>
        /// The debug.
        /// </value>
        bool? Debug { get; set; }

        /// <summary>
        /// Gets or sets the retention days.
        /// </summary>
        /// <value>
        /// The retention days.
        /// </value>
        int? RetentionDays { get; set; }

        /// <summary>
        /// Gets or sets the log file limit in bytes.
        /// </summary>
        /// <value>
        /// The log file limit in bytes.
        /// </value>
        int? LogFileLimitInBytes { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        string? Template { get; set; }

        /// <summary>
        /// Gets or sets the seq endpoint.
        /// </summary>
        /// <value>
        /// The seq endpoint.
        /// </value>
        string? SeqEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        string? ApplicationName { get; set; }
    }
}