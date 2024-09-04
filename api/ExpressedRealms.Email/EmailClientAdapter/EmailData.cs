namespace ExpressedRealms.Email.EmailClientAdapter;

internal record EmailData(string ToField, string Subject, string PlainTextBody, string HtmlBody);
