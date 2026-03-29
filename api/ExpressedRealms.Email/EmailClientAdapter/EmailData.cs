namespace ExpressedRealms.Email.EmailClientAdapter;

public record EmailData(string ToField, string Subject, string PlainTextBody, string HtmlBody);
