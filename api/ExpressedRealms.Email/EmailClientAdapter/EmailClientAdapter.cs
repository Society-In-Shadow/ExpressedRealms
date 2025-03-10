using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Microsoft.Extensions.Logging;
using PostmarkDotNet;

namespace ExpressedRealms.Email.EmailClientAdapter;

internal sealed class EmailClientAdapter(
    ILogger<EmailClientAdapter> logger,
    IKeyVaultManager keyVaultManager
) : IEmailClientAdapter
{
    public async Task SendEmailAsync(EmailData data)
    {
        var message = new PostmarkMessage
        {
            From = await keyVaultManager.GetSecret(EmailSettings.NoReplyEmail),
            To = data.ToField,
            Subject = data.Subject,
            TextBody = data.PlainTextBody,
            HtmlBody = data.HtmlBody,
        };

        var client = new PostmarkClient(await keyVaultManager.GetSecret(EmailSettings.Postmark));

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
