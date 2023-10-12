namespace bs.Frmwrk.Core.Models.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public interface IEmailSettings
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        string? From { get; set; }

        /// <summary>
        /// Gets or sets from display name.
        /// </summary>
        /// <value>
        /// From display name.
        /// </value>
        string? FromDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the ignore SSL validity.
        /// </summary>
        /// <value>
        /// The ignore SSL validity.
        /// </value>
        bool? IgnoreSSLValidity { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string? Password { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>
        /// The SMTP server.
        /// </value>
        string? SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the use SSL.
        /// </summary>
        /// <value>
        /// The use SSL.
        /// </value>
        bool? UseSSL { get; set; }
    }
}