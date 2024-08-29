using ExpressedRealms.Email.EmailClientAdapter;
using Microsoft.Extensions.Configuration;

namespace ExpressedRealms.Email.TestEmail;

internal class TestEmail(IEmailClientAdapter emailClientClient, IConfiguration configuration)
    : ITestEmail
{
    public async Task SendTestEmail()
    {
        await emailClientClient.SendEmailAsync(
            new EmailData(
                configuration["TEST_EMAIL_ADDRESS"],
                "This is a test email",
                "Test body",
                "Test <i>Body<i>"
            )
        );
    }
}
