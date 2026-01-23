using PdfSharp.Fonts;

namespace ExpressedRealms.Characters.Reports;

public sealed class MultiFontResolver : IFontResolver
{
    private static readonly string BasePath = Path.Combine(AppContext.BaseDirectory, "fonts");

    private static string FontFile(string subfolder, string fileName) =>
        Path.Combine(BasePath, subfolder, fileName);

    /// <summary>
    /// Resolve Typeface feeds into here
    /// </summary>
    public byte[] GetFont(string faceName)
    {
        const string liberationFolder = "liberation";
        const string dejavuFolder = "dejavu";

        if (faceName.Contains("Liberation"))
        {
            return File.ReadAllBytes(FontFile(liberationFolder, $"{faceName}.ttf"));
        }

        if (faceName.Contains("DejaVu"))
        {
            return File.ReadAllBytes(FontFile(dejavuFolder, $"{faceName}.ttf"));
        }

        throw new InvalidOperationException($"Unknown font '{faceName}'");
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
    {
        familyName = familyName.Trim();

        var fontStyle = string.Empty;
        if (bold && italic)
        {
            fontStyle = "-BoldItalic";
        }
        else if (bold)
        {
            fontStyle = "-Bold";
        }
        else if (italic)
        {
            fontStyle = "-Italic";
        }

        // Map PDF-declared fonts to your bundled fonts
        return familyName switch
        {
            "Helvetica" => new FontResolverInfo("LiberationSans"),
            "Helvetica-Bold" => new FontResolverInfo("LiberationSans-Bold"),
            "Courier New" or "Lucida Console" => new FontResolverInfo($"LiberationMono{fontStyle}"),
            "Arial" or "Segoe UI" or "SegoeWP" or "Verdana" => new FontResolverInfo(
                $"LiberationSans{fontStyle}"
            ),
            "Times New Roman" => new FontResolverInfo($"LiberationSerif{fontStyle}"),

            "MonoFallback" => new FontResolverInfo($"DejaVuSansMono{fontStyle}"),
            "SansFallback" => new FontResolverInfo($"DejaVuSans{fontStyle}"),
            "SerifFallback" => new FontResolverInfo($"DejaVuSerif{fontStyle}"),

            _ => new FontResolverInfo("DejaVuSans"),
        };
    }
}
