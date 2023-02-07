using bs.Frmwrk.Core.Dtos.Mailing;

namespace bs.Frmwrk.Mailing.Dtos
{
    public class MailMessageDto : IMailMessageDto
    {
        public string[]? Attachments { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool BodyHtml { get; set; }
        public string[]? CcEmails { get; set; }
        public string[]? CcnEmails { get; set; }
        public bool IsHtmlBody { get; set; }
        public MessagePriority Priority { get; set; } = MessagePriority.Normal;
        public string? SenderDisplayName { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; }
        public string[]? ToEmails { get; set; }
    }
}