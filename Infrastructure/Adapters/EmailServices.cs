using Application.Adapters;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Adapters;

/// <summary>
///  Email services
/// </summary>
public class EmailServices : IEmailServices
{
    private readonly ILogger<EmailServices> _logger;

    public EmailServices(ILogger<EmailServices> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///  Send email with template id with argument pass for form 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="content"></param>
    /// <param name="subject"></param>
    /// <param name="isHtml"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="useSsl"></param>
    /// <param name="applicationPassword"></param>
    /// <param name="nameTo"></param>
    /// <param name="nameFrom"></param>
    public async Task SendEmailAsync(string from, string to, string? content, string subject, bool isHtml, string host,
        int port, bool useSsl, string applicationPassword, string? nameTo = null, string? nameFrom = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            var emailForm = new MailboxAddress(nameFrom, from);
            emailMessage.From.Add(emailForm);
            var emailTo = new MailboxAddress(nameTo, to);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = content;
            }
            else
            {
                bodyBuilder.TextBody = content;
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(host, port, useSsl);
            await mailClient.AuthenticateAsync(nameFrom, applicationPassword);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            throw;
        }
    }
}