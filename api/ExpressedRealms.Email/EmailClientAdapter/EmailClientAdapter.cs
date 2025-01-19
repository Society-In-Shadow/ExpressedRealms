using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostmarkDotNet;

namespace ExpressedRealms.Email.EmailClientAdapter;

internal sealed class EmailClientAdapter(
    IConfiguration configuration,
    ILogger<EmailClientAdapter> logger
) : IEmailClientAdapter
{
    public async Task SendEmailAsync(EmailData data)
    {
        var message = new PostmarkMessage
        {
            From = configuration["NO_REPLY_EMAIL"],
            To = data.ToField,
            Subject = data.Subject,
            TextBody = data.PlainTextBody,
            HtmlBody = data.HtmlBody,
        };

        var client = new PostmarkClient(configuration["POSTMARK_API_KEY"]);

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
