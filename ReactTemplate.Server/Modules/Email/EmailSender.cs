using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReactTemplate.Server.Modules.Users;

namespace ReactTemplate.Server.Modules.Email;

public sealed class EmailSender(IOptions<SmtpOptions> options, ILogger<EmailSender> logger, IConfiguration configuration) : IEmailSender<User>
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        var username = configuration["SMTP_USERNAME"];
        var password = configuration["SMTP_PASSWORD"];

        logger.LogInformation("Attempting to send email: To={Email}, Subject={Subject}", toEmail, subject);
        logger.LogInformation("SMTP Config: Server={Server}, Port={Port}, SSL={EnableSSL}", options.Value.Server, options.Value.Port, options.Value.EnableSSL);
        logger.LogInformation("SMTP Username set: {HasUsername}", !string.IsNullOrEmpty(username));

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("SMTP_USERNAME and SMTP_PASSWORD must be configured. For Gmail, use an App Password (not your regular password).");
        }

        try
        {
            using SmtpClient client = new(options.Value.Server, options.Value.Port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = options.Value.EnableSSL,
                Timeout = 10000
            };

            using MailMessage msg = new() { From = new MailAddress(username) };

            msg.To.Add(toEmail);
            msg.Body = message;
            msg.Subject = subject;
            msg.IsBodyHtml = true;

            logger.LogInformation("Sending email via SMTP...");
            await client.SendMailAsync(msg);
            logger.LogInformation("Email sent successfully to {Email}", toEmail);
        }
        catch (SmtpException ex)
        {
            logger.LogError(ex, "SMTP error sending email to {Email}. Status: {StatusCode}", toEmail, ex.StatusCode);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Email}. Server: {Server}:{Port}, SSL: {EnableSSL}", toEmail, options.Value.Server, options.Value.Port, options.Value.EnableSSL);
            throw;
        }
    }

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "Welcome! Confirm your account";
        var body = $@"
            <h1>Hello {user.UserName},</h1>
            <p>Thank you for signing up. Click the link below to activate your account:</p>
            <p><a href='{confirmationLink}'>Confirm my account</a></p>";

        await Execute(subject, body, email);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var subject = "Password Reset";
        var body = $@"
            <p>To reset your password, click here:</p>
            <p><a href='{resetLink}'>Reset Password</a></p>";

        await Execute(subject, body, email);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "Password Reset Code";
        var body = $"Your reset code is: {resetCode}";

        await Execute(subject, body, email);
    }
}
