using MimeKit;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;

namespace MolineMart.API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config) => _config = config;

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtp = new System.Net.Mail.SmtpClient(_config["SmtpSettings:Host"], int.Parse(_config["SmtpSettings:Port"]))
            {
                Credentials = new NetworkCredential(_config["SmtpSettings:User"], _config["SmtpSettings:Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(_config["SmtpSettings:User"]),
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            await smtp.SendMailAsync(mail);
        }

        public async Task SendWebhookEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:User"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Host"], int.Parse(_config["SmtpSettings:Port"]), false);
            await smtp.AuthenticateAsync(_config["SmtpSettings:User"], _config["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
