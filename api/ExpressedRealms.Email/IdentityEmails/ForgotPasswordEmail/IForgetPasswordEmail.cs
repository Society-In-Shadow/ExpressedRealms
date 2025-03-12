namespace ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;

public interface IForgetPasswordEmail
{
    Task<(string subject, string plaintext, string html)> GetUpdatedEmailTemplate(
        string htmlContent
    );
}
