using ExpressedRealms.Email.EmailClientAdapter;
using ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;
using ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ExpressedRealms.Email.IdentityEmails;

internal sealed class IdentityEmailSender(
    IForgetPasswordEmail forgetPasswordEmail,
    IConfirmAccountEmail confirmAccountEmail,
    IEmailClientAdapter emailClientAdapter
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

        await emailClientAdapter.SendEmailAsync(
            new EmailData(email, subject, plainTextMessage, htmlMessage)
        );
    }
}
