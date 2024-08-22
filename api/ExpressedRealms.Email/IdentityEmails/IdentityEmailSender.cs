using ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;
using ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

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
        
        await emailClientAdapter.SendEmailAsync(new EmailData(
            email,
            subject,
            plainTextMessage,
            htmlMessage));
    }
}
