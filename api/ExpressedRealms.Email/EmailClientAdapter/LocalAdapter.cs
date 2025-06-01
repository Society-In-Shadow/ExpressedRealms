using System.Net.Mail;
using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Microsoft.Extensions.Logging;

namespace ExpressedRealms.Email.EmailClientAdapter;

internal sealed class LocalAdapter(
    ILogger<EmailClientAdapter> logger,
    IKeyVaultManager keyVaultManager
) : IEmailClientAdapter
{
    public async Task SendEmailAsync(EmailData data)
    {
        var fromEmail = new MailAddress(
            await keyVaultManager.GetSecret(EmailSettings.NoReplyEmail)
        );
        var toEmail = new MailAddress(data.ToField);

        using var message = new MailMessage(fromEmail, toEmail);
        message.Subject = data.Subject;
        message.Body = data.HtmlBody;
        message.IsBodyHtml = true;

        var serverAddress = Environment.GetEnvironmentVariable("SMTP-SERVER");

        using var client = new SmtpClient(serverAddress.Split(':')[0]);
        client.Port = int.Parse(serverAddress.Split(':')[1]);

        await client.SendMailAsync(message);
        logger.LogTrace("Successfully sent message!");
    }
}
