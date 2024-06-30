using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExpressedRealms.Email.SendGridTestEmail;

internal class SendGridEmail(ISendGridClient sendGridClient, IConfiguration configuration)
    : ISendGridEmail
{
    public async Task SendTestEmail()
    {
        var from_email = new EmailAddress(configuration["FROM_EMAIL"], "Example User");
        var subject = "Sending with Twilio SendGrid is Fun";
        var to_email = new EmailAddress("noremacskich@gmail.com", "Example User");
        var plainTextContent = "and easy to do anywhere, even with C#";
        var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        var msg = MailHelper.CreateSingleEmail(
            from_email,
            to_email,
            subject,
            plainTextContent,
            htmlContent
        );
        await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
    }
}
