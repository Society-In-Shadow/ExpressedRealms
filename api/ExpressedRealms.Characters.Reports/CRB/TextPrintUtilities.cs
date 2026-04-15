using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ExpressedRealms.Characters.Reports.CRB;

internal static class TextPrintUtilities
{
    public const string DefaultFontFace = "Liberation Sans";

    public static void Print90DegreeMessage(
        XGraphics gfx,
        string stampText,
        double centerX,
        double centerY,
        XSolidBrush color
    )
    {
        var font = new XFont(DefaultFontFace, 10, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(stampText, font, color, -size.Width / 2, font.GetHeight() / 2 - 3);
        gfx.Restore();
    }

    public static void CrossStampInfo(
        PdfPage page,
        string stampText,
        double centerX,
        double centerY
    )
    {
        using var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont(DefaultFontFace, 12, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(
            stampText,
            font,
            XBrushes.Black,
            -size.Width / 2,
            font.GetHeight() / 2 - XUnitPt.FromInch(0.05)
        );
        gfx.Restore();
    }

    public static void PrintStatInfo(PdfPage page, string stampText, double centerX, double centerY)
    {
        using var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont(DefaultFontFace, 10, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(stampText, font, XBrushes.Black, -size.Width / 2, font.GetHeight() / 2 - 3);
        gfx.Restore();
    }

    public static void PrintStatLabelInfo(
        PdfPage page,
        string label,
        double centerX,
        double centerY
    )
    {
        using var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont(DefaultFontFace, 10, XFontStyleEx.Regular);
        var size = gfx.MeasureString(label, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        // After -90°: width is horizontal centering, ascent pins the baseline to the bottom
        gfx.DrawString(label, font, XBrushes.Black, -size.Height - 2, 0);

        gfx.Restore();

        var linePen = new XPen(XColors.Black, XUnitPt.FromInch(0.015));
        gfx.DrawLine(
            linePen,
            XUnitPt.FromInch(4.16),
            XUnitPt.FromInch(4.31),
            XUnitPt.FromInch(4.16),
            XUnitPt.FromInch(4.82)
        );
    }

    public static void PrintPPIdentifier(PdfPage page, double centerX, double centerY)
    {
        using var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont(DefaultFontFace, 4, XFontStyleEx.Regular);
        var size = gfx.MeasureString("PP", font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-180);
        gfx.DrawString("PP", font, XBrushes.Black, -size.Width / 2, font.GetHeight() / 2 - 3);
        gfx.Restore();
    }

    public static void PrintSkills(XGraphics gfx, string stampText, double centerX, double centerY)
    {
        var font = new XFont(DefaultFontFace, 9, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(stampText, font, XBrushes.Black, -size.Width / 2, font.GetHeight() / 2 - 3);
        gfx.Restore();
    }
}
