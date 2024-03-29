﻿using bs.Frmwrk.Core.Dtos.Mailing;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mailing;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MessagePriority = bs.Frmwrk.Core.Dtos.Mailing.MessagePriority;

namespace bs.Frmwrk.Mailing.Services
{
    public class MailingService : IMailingService
    {
        private readonly IEmailSettings emailSettings;
        private readonly ILogger<MailingService> logger;
        private readonly ITranslateService translateService;

        public MailingService(ILogger<MailingService> logger, ITranslateService translateService, IEmailSettings emailSettings)
        {
            this.logger = logger;
            this.translateService = translateService;
            this.emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(IMailMessageDto messageDto)
        {
            var message = CreateEmailMessage(messageDto);
            using var client = new SmtpClient();
            try
            {
                if (emailSettings.IgnoreSSLValidity ?? false)
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.Port, emailSettings.UseSSL ?? false).ConfigureAwait(false);
                if (!(emailSettings.DisableAuthentication ?? false))
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailSettings.UserName, emailSettings.Password);
                }

                var response = await client.SendAsync(message);
                logger.LogDebug($"SMTP server response is:\n{response}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending email");
                throw new BsException(2212220933, translateService.Translate($"Impossibile inviare l'email: {ex.GetBaseException().Message}"), ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage CreateEmailMessage(IMailMessageDto messageDto)
        {
            var smtpEmailMessage = new MimeMessage();
            smtpEmailMessage.From.Add(new MailboxAddress(emailSettings.FromDisplayName ?? "", emailSettings.From));
            if (messageDto.ToEmails != null) smtpEmailMessage.To.AddRange(messageDto.ToEmails.Select(m => new MailboxAddress("", m)));
            if (messageDto.CcEmails != null) smtpEmailMessage.Cc.AddRange(messageDto.CcEmails.Select(m => new MailboxAddress("", m)));
            if (messageDto.CcnEmails != null) smtpEmailMessage.Bcc.AddRange(messageDto.CcnEmails.Select(m => new MailboxAddress("", m)));
            smtpEmailMessage.Subject = messageDto.Subject;
            smtpEmailMessage.Body = new TextPart(messageDto.IsHtmlBody ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Text) { Text = messageDto.Body };
            switch (messageDto.Priority)
            {
                case MessagePriority.BelowNormal:
                    smtpEmailMessage.Priority = MimeKit.MessagePriority.NonUrgent;

                    break;

                case MessagePriority.Normal:
                    smtpEmailMessage.Priority = MimeKit.MessagePriority.Normal;

                    break;

                case MessagePriority.Urgent:
                    smtpEmailMessage.Priority = MimeKit.MessagePriority.Urgent;
                    break;

                default:
                    break;
            }
            return smtpEmailMessage;
        }
    }
}