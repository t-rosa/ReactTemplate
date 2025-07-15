using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ReactTemplate.Server.Services
{
    public class EmailSender(IOptions<SmtpOptions> options, ILogger<EmailSender> logger, IConfiguration configuration) : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            var username = configuration["Smtp:Username"];
            var password = configuration["Smtp:Password"];

            if (username == null && password == null)
            {
                throw new Exception();
            }

            using SmtpClient client = new(options.Value.Server, options.Value.Port);
            client.Credentials = new NetworkCredential(
                username,
                password
            );
            client.EnableSsl = options.Value.EnableSSL;

            MailMessage msg = new() { From = new MailAddress(username!) };

            msg.To.Add(toEmail);
            msg.Body = message;
            msg.Subject = subject;
            msg.IsBodyHtml = true;

            try
            {
                await client.SendMailAsync(msg);
                logger.LogInformation($"Email sent to {toEmail}");
            }
            catch
            {
                logger.LogError($"Failed to send email to {toEmail}");
            }
        }
    }
}
