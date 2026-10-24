using HtmlAgilityPack;

namespace ExpressedRealms.UseCases.Shared;

public static class HtmlContentProcessor
{
    public static string ToPlainText(this string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        var document = new HtmlDocument();
        document.LoadHtml(html);

        document.DocumentNode
            .SelectNodes("//script|//style")
            ?.ToList()
            .ForEach(node => node.Remove());

        return HtmlEntity.DeEntitize(
            document.DocumentNode.InnerText).Trim();
    }
}