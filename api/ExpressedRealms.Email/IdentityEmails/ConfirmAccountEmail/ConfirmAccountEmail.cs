using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;

namespace ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;

internal sealed class ConfirmAccountEmail(IKeyVaultManager keyVault) : IConfirmAccountEmail
{
    private string ParseAccountConfirmationLink(string identityEmail)
    {
        var url = identityEmail.Split("'")[1];
        return url.Split('?').Last();
    }

    public async Task<(string subject, string plaintext, string html)> GetUpdatedEmailTemplate(
        string htmlContent
    )
    {
        var subject = "Society in Shadows Account Confirmation";
        var confirmAccountParamters = ParseAccountConfirmationLink(htmlContent);
        var baseURL = await keyVault.GetSecret(EmailSettings.FrontEndBaseUrl);
        var plainTextContext =
            $@"Welcome to Society in Shadows!  Please copy and paste the link below to confirm your account

{baseURL}/confirmAccount?{confirmAccountParamters.Replace("&amp;", "&")}

Thanks,
Society in Shadows";

        string htmlEmail = $"""
            <p>Welcome to Society in Shadows!  Please click the link below to confirm your account</p>

            <p><a href="{baseURL}/confirmAccount?{confirmAccountParamters}">Confirm Account</a></p>

            <p>Thanks,</p>
            <p>Society in Shadows</p>
            """;

        return (subject, plainTextContext, htmlEmail);
    }
}
