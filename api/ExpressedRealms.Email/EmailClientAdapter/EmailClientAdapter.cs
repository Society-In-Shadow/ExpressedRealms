using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.Extensions.Logging;
using PostmarkDotNet;

namespace ExpressedRealms.Email.EmailClientAdapter;

internal sealed class EmailClientAdapter(ILogger<EmailClientAdapter> logger) : IEmailClientAdapter
{
    public async Task SendEmailAsync(EmailData data)
    {
        var message = new PostmarkMessage
        {
            From = KeyVaultManager.GetSecret(EmailSettings.NoReplyEmail),
            To = data.ToField,
            Subject = data.Subject,
            TextBody = data.PlainTextBody,
            HtmlBody = data.HtmlBody,
        };

        var client = new PostmarkClient(KeyVaultManager.GetSecret(EmailSettings.Postmark));

        var response = await client.SendMessageAsync(message);

        if (response.Status == PostmarkStatus.Success)
        {
            logger.LogTrace("Successfully sent message!");
        }
        else
        {
            logger.LogError(
                "Email did not send.  Error Code {errorCode}.  Message {message}",
                response.ErrorCode,
                response.Message
            );
        }
    }
}
