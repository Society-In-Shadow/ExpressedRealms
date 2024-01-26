using Microsoft.Extensions.Configuration;

namespace ExpressedRealms.Email.IdentityEmails;

internal class ForgetPasswordEmail(IConfiguration configuration) : IForgetPasswordEmail
{
    private string ParseResetToken(string identityEmail)
    {
        return identityEmail.Split(" ").Last();
    }
    
    public (string subject, string plaintext, string html) GetUpdatedEmailTemplate(string htmlContent)
    {
        var subject = "Society in Shadows Password Reset";
        var resetToken = ParseResetToken(htmlContent);
        var plainTextContext =  
            $@"You recently requested to reset the password for your Society in Shadows account. Copy and paste the link below to proceed.

{configuration["frontendBaseURL"]}/resetpassword?resetToken={resetToken}

If you did not request a password reset, please ignore this email.
This password reset link is only valid for the next 30 minutes.

Thanks,
Society in Shadows";

        string htmlEmail = 
            $"""
            <p>You recently requested to reset the password for your Society in Shadows account. Click the button below to proceed.</p>

            <p><a href="{configuration["frontendBaseURL"]}/resetpassword?resetToken={resetToken}"> Reset Password </a></p>

            <p>If you did not request a password reset, please ignore this email.</p>
            <p>This password reset link is only valid for the next 30 minutes.</p>

            <p>Thanks,</p>
            <p>Society in Shadows</p>
            """;

        return (subject, plainTextContext, htmlEmail);
    }
}