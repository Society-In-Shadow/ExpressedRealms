namespace ExpressedRealms.Email.EmailClientAdapter;

internal interface IEmailClientAdapter
{
    Task SendEmailAsync(EmailData data);
}
