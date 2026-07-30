namespace ExpressedRealms.Shared;

public static class StringExtensions
{
    public static string Limit(this string? value, int maxLength, string terminator = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value ?? string.Empty;

        if (maxLength <= terminator.Length)
            return terminator[..maxLength];

        return value[..(maxLength - terminator.Length)] + terminator;
    }
}
