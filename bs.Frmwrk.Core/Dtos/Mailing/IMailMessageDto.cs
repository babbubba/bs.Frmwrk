namespace bs.Frmwrk.Core.Dtos.Mailing
{
    /// <summary>
    ///
    /// </summary>
    public interface IMailMessageDto
    {
        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>
        /// The attachments.
        /// </value>
        string[]? Attachments { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [body HTML].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [body HTML]; otherwise, <c>false</c>.
        /// </value>
        bool BodyHtml { get; set; }

        /// <summary>
        /// Gets or sets the cc emails.
        /// </summary>
        /// <value>
        /// The cc emails.
        /// </value>
        string[]? CcEmails { get; set; }

        /// <summary>
        /// Gets or sets the CCN emails.
        /// </summary>
        /// <value>
        /// The CCN emails.
        /// </value>
        string[]? CcnEmails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is HTML body.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is HTML body; otherwise, <c>false</c>.
        /// </value>
        bool IsHtmlBody { get; set; }

        /// <summary>
        /// Gets or sets the display name of the sender.
        /// </summary>
        /// <value>
        /// The display name of the sender.
        /// </value>
        string? SenderDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        string Subject { get; set; }

        /// <summary>
        /// Converts to emails.
        /// </summary>
        /// <value>
        /// To emails.
        /// </value>
        string[]? ToEmails { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        MessagePriority Priority { get; set; }
    }
}