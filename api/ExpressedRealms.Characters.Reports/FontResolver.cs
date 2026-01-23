using PdfSharp.Fonts;

namespace ExpressedRealms.Characters.Reports;

public sealed class MultiFontResolver : IFontResolver
{
    private static readonly string BasePath = Path.Combine(AppContext.BaseDirectory, "fonts");

    private static string FontFile(string subfolder, string fileName)
        => Path.Combine(BasePath, subfolder, fileName);

    public byte[] GetFont(string faceName)
    {
        // Map the face name to the actual file
        string filePath = faceName switch
        {
            "LiberationMono" => FontFile("liberation", "LiberationMono-Regular.ttf"),
            "LiberationMono-Bold" => FontFile("liberation", "LiberationMono-Bold.ttf"),
            "LiberationSans" => FontFile("liberation", "LiberationSans-Regular.ttf"),
            "LiberationSans-Bold" => FontFile("liberation", "LiberationSans-Bold.ttf"),
            "LiberationSerif" => FontFile("liberation", "LiberationSerif-Regular.ttf"),
            "LiberationSerif-Bold" => FontFile("liberation", "LiberationSerif-Bold.ttf"),

            "DejaVuSansMono" => FontFile("dejavu", "DejaVuSansMono.ttf"),
            "DejaVuSansMono-Bold" => FontFile("dejavu", "DejaVuSansMono-Bold.ttf"),
            "DejaVuSans" => FontFile("dejavu", "DejaVuSans.ttf"),
            "DejaVuSans-Bold" => FontFile("dejavu", "DejaVuSans-Bold.ttf"),
            "DejaVuSerif" => FontFile("dejavu", "DejaVuSerif.ttf"),
            "DejaVuSerif-Bold" => FontFile("dejavu", "DejaVuSerif-Bold.ttf"),

            _ => throw new InvalidOperationException($"Unknown font '{faceName}'")
        };

        return File.ReadAllBytes(filePath);
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        familyName = familyName.Trim();

        // Map PDF-declared fonts to your bundled fonts
        return familyName switch
        {
            "Helvetica" => new FontResolverInfo("LiberationSans"),
            "Helvetica-Bold" => new FontResolverInfo("LiberationSans-Bold"),
            "Courier New" or "Lucida Console" => new FontResolverInfo(
                isBold ? "LiberationMono-Bold" : "LiberationMono"),

            "Arial" or "Segoe UI" or "SegoeWP" or "Verdana" =>
                new FontResolverInfo(isBold ? "LiberationSans-Bold" : "LiberationSans"),

            "Times New Roman" => new FontResolverInfo(
                isBold ? "LiberationSerif-Bold" : "LiberationSerif"),

            // Optionally: use DejaVu for monospaced or general fallback
            "MonoFallback" => new FontResolverInfo(
                isBold ? "DejaVuSansMono-Bold" : "DejaVuSansMono"),

            "SansFallback" => new FontResolverInfo(
                isBold ? "DejaVuSans-Bold" : "DejaVuSans"),

            "SerifFallback" => new FontResolverInfo(
                isBold ? "DejaVuSerif-Bold" : "DejaVuSerif"),

            // Symbols / unrecognized fonts
            "Symbol" or "Wingdings" or _ => new FontResolverInfo("DejaVuSans")
        };
    }
}