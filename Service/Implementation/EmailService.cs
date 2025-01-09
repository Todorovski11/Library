using Domain.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid.Helpers.Mail;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(EmailMessage allMails)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress("LibraryStore Application", "no-reply@librarystore.com"),
                Subject = allMails.Subject
            };

            emailMessage.From.Add(new MailboxAddress("LibraryStore Application", "no-reply@librarystore.com"));
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = allMails.Content };
            emailMessage.To.Add(new MailboxAddress(allMails.MailTo, allMails.MailTo));

            try
            {
                //using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                //{
                //    var socketOptions = MailKit.Security.SecureSocketOptions.Auto;
                //    await smtp.ConnectAsync(_mailSettings.SmtpServer, 587, socketOptions);

                //    if (!string.IsNullOrEmpty(_mailSettings.SmtpUserName))
                //    {
                //        await smtp.AuthenticateAsync(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                //    }

                //    await smtp.SendAsync(emailMessage);
                //    await smtp.DisconnectAsync(true);
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
