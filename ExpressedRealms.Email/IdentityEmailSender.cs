using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExpressedRealms.Email;

public class IdentityEmailSender : IEmailSender
{
    private readonly ISendGridClient _sendGrid;

    public IdentityEmailSender(ISendGridClient sendGrid)
    {
        _sendGrid = sendGrid;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {

        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress("test123@example.com"), 
            new EmailAddress(email), 
            subject, 
            "", 
            htmlMessage);
        var response = await _sendGrid.SendEmailAsync(msg).ConfigureAwait(false);
    }
}