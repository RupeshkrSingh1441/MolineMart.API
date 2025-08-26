namespace MolineMart.API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendWebhookEmailAsync(string to, string subject, string body);
    }
}
