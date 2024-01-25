using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExpressedRealms.Email;

internal class SendGridEmail : ISendGridEmail
{
    private readonly ISendGridClient _sendGridClient;

    public SendGridEmail(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
    }

    public async Task SendTestEmail()
    {
        var from_email = new EmailAddress("test@example.com", "Example User");
        var subject = "Sending with Twilio SendGrid is Fun";
        var to_email = new EmailAddress("noremacskich@gmail.com", "Example User");
        var plainTextContent = "and easy to do anywhere, even with C#";
        var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, htmlContent);
        var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
    }
}