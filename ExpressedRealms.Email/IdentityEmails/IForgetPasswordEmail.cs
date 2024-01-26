namespace ExpressedRealms.Email.IdentityEmails;

public interface IForgetPasswordEmail
{
    (string subject, string plaintext, string html) GetUpdatedEmailTemplate(string htmlContent);
}