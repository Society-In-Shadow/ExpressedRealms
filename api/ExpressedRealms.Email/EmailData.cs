namespace ExpressedRealms.Email;

public record EmailData(
    string ToField,
    string Subject,
    string PlainTextBody,
    string HtmlBody
);