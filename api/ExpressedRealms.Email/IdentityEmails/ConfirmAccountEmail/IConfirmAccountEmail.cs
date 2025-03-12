namespace ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;

internal interface IConfirmAccountEmail
{
    Task<(string subject, string plaintext, string html)> GetUpdatedEmailTemplate(
        string htmlContent
    );
}
