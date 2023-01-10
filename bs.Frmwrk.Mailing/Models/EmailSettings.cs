using bs.Frmwrk.Core.Models.Configuration;

namespace bs.Frmwrk.Mailing.Models
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Models.Configuration.IEmailSettings" />
    public class EmailSettings : IEmailSettings
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string? From { get; set; }

        /// <summary>
        /// Gets or sets from display name.
        /// </summary>
        /// <value>
        /// From display name.
        /// </value>
        public string? FromDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>
        /// The SMTP server.
        /// </value>
        public string? SmtpServer { get; set; }


        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; } = 25;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }
    }
}