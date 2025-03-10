using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;

namespace ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;

internal sealed class ForgetPasswordEmail(IKeyVaultManager keyVault) : IForgetPasswordEmail
{
    private string ParseResetToken(string identityEmail)
    {
        return identityEmail.Split(" ").Last();
    }

    public async Task<(string subject, string plaintext, string html)> GetUpdatedEmailTemplate(
        string htmlContent
    )
    {
        var subject = "Society in Shadows Password Reset";
        var resetToken = ParseResetToken(htmlContent);
        var baseURL = await keyVault.GetSecret(EmailSettings.FrontEndBaseUrl);
        var plainTextContext =
            $@"You recently requested to reset the password for your Society in Shadows account. Copy and paste the link below to proceed.

{baseURL}/resetpassword?resetToken={resetToken}

If you did not request a password reset, please ignore this email.
This password reset link is only valid for the next 24 hours.

Thanks,
Society in Shadows";

        string htmlEmail = $"""
            <p>You recently requested to reset the password for your Society in Shadows account. Click the button below to proceed.</p>

            <p><a href="{baseURL}/resetpassword?resetToken={resetToken}"> Reset Password </a></p>

            <p>If you did not request a password reset, please ignore this email.</p>
            <p>This password reset link is only valid for the next 30 minutes.</p>

            <p>Thanks,</p>
            <p>Society in Shadows</p>
            """;

        return (subject, plainTextContext, htmlEmail);
    }
}
