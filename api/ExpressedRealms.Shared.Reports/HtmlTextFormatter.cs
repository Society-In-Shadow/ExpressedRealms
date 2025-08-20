using System.Text.RegularExpressions;

namespace ExpressedRealms.Shared.Reports;

public static class HtmlTextFormatter
{
    private static readonly Regex TdContentRegex = new Regex(
        "(<td\\b[^>]*>)(.*?)(</td>)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5)
    );

    private static readonly Regex POpenRegex = new Regex(
        "<p\\b[^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5)
    );

    private static readonly Regex PCloseRegex = new Regex(
        "</p\\s*>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled,
        TimeSpan.FromSeconds(5)
    );

    // Removes <p...> and replaces </p> with a newline ONLY inside <td>...</td> content blocks
    public static string NormalizeParagraphsInsideTd(string html)
    {
        return TdContentRegex.Replace(
            html,
            m =>
            {
                var openTd = m.Groups[1].Value;
                var inner = m.Groups[2].Value;
                var closeTd = m.Groups[3].Value;

                inner = POpenRegex.Replace(inner, "");
                inner = PCloseRegex.Replace(inner, "\n");

                return openTd + inner + closeTd;
            }
        );
    }
}
