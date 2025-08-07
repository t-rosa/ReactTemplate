using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace ReactTemplate.Server.Modules.Email;

public class EmailSender : IEmailSender
{
    private readonly IOptions<SmtpOptions> _options;
    private readonly ILogger<EmailSender> _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(IOptions<SmtpOptions> options, ILogger<EmailSender> logger, IConfiguration configuration)
    {
        _options = options;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        var username = _configuration["SMTP_USERNAME"];
        var password = _configuration["SMTP_PASSWORD"];

        if (username == null && password == null)
        {
            throw new Exception();
        }

        using SmtpClient client = new(_options.Value.Server, _options.Value.Port);
        client.Credentials = new NetworkCredential(
            username,
            password
        );
        client.EnableSsl = _options.Value.EnableSSL;

        MailMessage msg = new() { From = new MailAddress(username!) };

        msg.To.Add(toEmail);
        msg.Body = message;
        msg.Subject = subject;
        msg.IsBodyHtml = true;

        try
        {
            await client.SendMailAsync(msg);
            _logger.LogInformation($"Email sent to {toEmail}");
        }
        catch
        {
            _logger.LogError($"Failed to send email to {toEmail}");
        }
    }
}
