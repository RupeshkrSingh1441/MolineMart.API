using System.Net;
using System.Net.Mail;

namespace MolineMart.API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config) => _config = config;

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtp = new SmtpClient(_config["SmtpSettings:Host"], int.Parse(_config["SmtpSettings:Port"]))
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
    }
}
