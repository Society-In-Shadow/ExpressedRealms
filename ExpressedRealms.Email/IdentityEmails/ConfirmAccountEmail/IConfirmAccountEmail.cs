namespace ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;

internal interface IConfirmAccountEmail
{
    (string subject, string plaintext, string html) GetUpdatedEmailTemplate(string htmlContent);
}