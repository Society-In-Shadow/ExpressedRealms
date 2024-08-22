namespace ExpressedRealms.Email;

public interface IEmailClientAdapter
{
    Task SendEmailAsync(EmailData data);
}