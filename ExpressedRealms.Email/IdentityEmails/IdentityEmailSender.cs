using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExpressedRealms.Email.IdentityEmails;

internal sealed class IdentityEmailSender(
    ISendGridClient sendGrid,
    IConfiguration configuration,
    IForgetPasswordEmail forgetPasswordEmail)
    : IEmailSender
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var plainTextMessage = "";
        if (subject == "Reset your password")
        {
            (subject, plainTextMessage, htmlMessage) = forgetPasswordEmail.GetUpdatedEmailTemplate(htmlMessage);
        }
        
        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress("test123@example.com"), 
            new EmailAddress(email), 
            subject, 
            plainTextMessage, 
            htmlMessage);
        var response = await sendGrid.SendEmailAsync(msg).ConfigureAwait(false);
    }


}