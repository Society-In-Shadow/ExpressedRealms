namespace ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;

public interface IForgetPasswordEmail
{
    (string subject, string plaintext, string html) GetUpdatedEmailTemplate(string htmlContent);
}
