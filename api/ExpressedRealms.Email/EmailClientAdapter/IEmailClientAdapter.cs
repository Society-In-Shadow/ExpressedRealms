namespace ExpressedRealms.Email.EmailClientAdapter;

public interface IEmailClientAdapter
{
    Task SendEmailAsync(EmailData data);
}
