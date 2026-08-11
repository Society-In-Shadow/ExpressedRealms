using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Shared;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ExpressedRealms.Characters.Reports.CRB.CrbPages;

public static class ContactsPage
{
    public static void FillInContacts(PdfDocument document, List<ContactInfo> dataPowers)
    {
        using var gfx = XGraphics.FromPdfPage(document.Pages[2]);

        const double stampSize = 0.67 * 72;
        const double rowHeight = 20;

        var x = 3.87 * 72;
        var y = 4.41 * 72;
        var width = 2.65 * 72;

        var textFont = new XFont("Arial", 9);
        var stampFont = new XFont("Arial", 36);

        foreach (var knowledge in dataPowers.Take(6))
        {
            // Knowledge description
            var text =
                $"{knowledge.Name.Limit(25, ".")} - "
                + $"{knowledge.KnowledgeName.Limit(30, ".")} - "
                + $"{knowledge.KnowledgeLevel}";

            var textRect = new XRect(x, y + 2, width, rowHeight);

            gfx.DrawString(text, textFont, XBrushes.Black, textRect, XStringFormats.CenterLeft);

            y += rowHeight + 5;

            // Stamps
            var slotWidth = width / 3;

            var stampTexts = new[]
            {
                string.Empty,
                knowledge.NumberOfUses >= 2 ? string.Empty : "X",
                knowledge.NumberOfUses >= 3 ? string.Empty : "X",
            };

            for (var i = 0; i < stampTexts.Length; i++)
            {
                var stampX = x + (slotWidth * i) + ((slotWidth - stampSize) / 2);

                CreateStamp(
                    gfx,
                    new XRect(stampX, y, stampSize, stampSize),
                    stampTexts[i],
                    stampFont
                );
            }

            y += stampSize + 2;
        }
    }

    private static void CreateStamp(XGraphics gfx, XRect rect, string stampText, XFont font)
    {
        gfx.DrawRectangle(XPens.Black, rect);

        if (!string.IsNullOrEmpty(stampText))
        {
            gfx.DrawString(stampText, font, XBrushes.Black, rect, XStringFormats.Center);
        }
    }
}
