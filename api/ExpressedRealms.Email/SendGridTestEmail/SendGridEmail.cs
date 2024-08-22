using Microsoft.Extensions.Configuration;

namespace ExpressedRealms.Email.SendGridTestEmail;

internal class SendGridEmail(IEmailClientAdapter emailClientClient, IConfiguration configuration)
    : ISendGridEmail
{
    public async Task SendTestEmail()
    {
        await emailClientClient.SendEmailAsync(new EmailData(
            configuration["TEST_EMAIL_ADDRESS"],
            "This is a test email",
            "Test body",
            "Test <i>Body<i>"));
    }
}
