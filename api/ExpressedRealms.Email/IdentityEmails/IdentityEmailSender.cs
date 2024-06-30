using ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;
using ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExpressedRealms.Email.IdentityEmails;

internal sealed class IdentityEmailSender(
    ISendGridClient sendGrid,
    IForgetPasswordEmail forgetPasswordEmail,
    IConfirmAccountEmail confirmAccountEmail,
    IConfiguration configuration
) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var plainTextMessage = "";
        (subject, plainTextMessage, htmlMessage) = subject switch
        {
            "Reset your password" => forgetPasswordEmail.GetUpdatedEmailTemplate(htmlMessage),
            "Confirm your email" => confirmAccountEmail.GetUpdatedEmailTemplate(htmlMessage),
            _ => (subject, plainTextMessage, htmlMessage)
        };

        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress(configuration["FROM_EMAIL"]),
            new EmailAddress(email),
            subject,
            plainTextMessage,
            htmlMessage
        );
        var response = await sendGrid.SendEmailAsync(msg).ConfigureAwait(false);
    }
}
