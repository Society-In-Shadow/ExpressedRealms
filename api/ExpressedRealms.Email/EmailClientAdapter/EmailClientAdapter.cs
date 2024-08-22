using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExpressedRealms.Email.EmailClientAdapter;

internal sealed class EmailClientAdapter(
    IConfiguration configuration, 
    ILogger<EmailClientAdapter> logger
) : IEmailClientAdapter
{
    public async Task SendEmailAsync(EmailData data)
    {
        MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, configuration["NO_REPLY_EMAIL"])
            .Property(Send.FromName, configuration["NO_REPLY_EMAIL_NAME"])
            .Property(Send.Subject, data.Subject)
            .Property(Send.TextPart, data.PlainTextBody)
            .Property(Send.HtmlPart, data.HtmlBody)
            .Property(Send.Recipients, new JArray {
                new JObject {
                    {"Email", data.ToField}
                }
            });
        
        var mailClient = new MailjetClient(
            configuration["MAILJET_API_KEY"], 
            configuration["MAILJET_API_PRIVATE_KEY"]
        );
        
        MailjetResponse response = await mailClient.PostAsync(request);
        if (!response.IsSuccessStatusCode)
            logger.LogError("Email \"{Subject}\" failed to send to \"{toField}\".  StatusCode {statusCode}, ErrorInfo {errorInfo}, ErrorMessage: {errorMessage}", 
                data.Subject, data.ToField, response.StatusCode, response.GetErrorInfo(), response.GetErrorMessage());
        
    }
}
