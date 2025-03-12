using ExpressedRealms.Email.EmailClientAdapter;

namespace ExpressedRealms.Email.TestEmail;

internal class TestEmail(IEmailClientAdapter emailClientClient) : ITestEmail
{
    public async Task SendTestEmail(string userEmail)
    {
        await emailClientClient.SendEmailAsync(
            new EmailData(userEmail, "This is a test email", "Test body", "Test <i>Body<i>")
        );
    }
}
