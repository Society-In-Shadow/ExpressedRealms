namespace ExpressedRealms.Email.IdentityEmails;

internal interface IConfirmAccountEmail
{
    (string subject, string plaintext, string html) GetUpdatedEmailTemplate(string htmlContent);
}