using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Shared;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ExpressedRealms.Characters.Reports.CRB.CrbPages;

public static class ContactsPage
{
    public static void FillInContacts(PdfDocument document, List<ContactInfo> data)
    {
        using var gfx = XGraphics.FromPdfPage(document.Pages[2]);

        const double stampSize = 0.67 * 72;
        const double rowHeight = 20;

        var x = 3.87 * 72;
        var y = 4.41 * 72;
        var width = 2.65 * 72;

        var textFont = new XFont("Arial", 9);
        var stampFont = new XFont("Arial", 36);

        for(var i = 0; i < 6; i++)
        {
            var knowledge = i < data.Count
                ? data[i]
                : null;

            DrawContactDetails(knowledge, x, y, width, rowHeight, gfx, textFont);
            
            y += rowHeight + 5;

            GenerateStampsForContact(width, knowledge, x, stampSize, gfx, y, stampFont);

            y += stampSize + 2;
        }
    }

    private static void GenerateStampsForContact(double width, ContactInfo? knowledge, double x, double stampSize,
        XGraphics gfx, double y, XFont stampFont)
    {
        var slotWidth = width / 3;

        var stampTexts = new[] { string.Empty, string.Empty, string.Empty };
            
        if(knowledge is not null)
            stampTexts =
            [
                string.Empty,
                knowledge.NumberOfUses >= 2 ? string.Empty : "X",
                knowledge.NumberOfUses >= 3 ? string.Empty : "X"
            ];

        for (var j = 0; j < stampTexts.Length; j++)
        {
            var stampX = x + (slotWidth * j) + ((slotWidth - stampSize) / 2);

            CreateStamp(
                gfx,
                new XRect(stampX, y, stampSize, stampSize),
                stampTexts[j],
                stampFont
            );
        }
    }

    private static void DrawContactDetails(ContactInfo? knowledge, double x, double y, double width, double rowHeight,
        XGraphics gfx, XFont textFont)
    {
        if (knowledge is not null)
        {
            var text =
                $"{knowledge.Name.Limit(25, ".")} - "
                + $"{knowledge.KnowledgeName.Limit(30, ".")} - "
                + $"{knowledge.KnowledgeLevel}";

            var textRect = new XRect(x, y + 2, width, rowHeight);

            gfx.DrawString(text, textFont, XBrushes.Black, textRect, XStringFormats.CenterLeft);
        }
        else
        {
            var textRect = new XRect(x, y + 2, width, rowHeight);
            
            gfx.DrawString("Name - Knowledge - Level (Cross out Unusable Boxes Below)", new XFont("Arial", 7), XBrushes.LightGray, textRect, XStringFormats.CenterLeft);
            
            var linePen = new XPen(XColors.Black, 0.5);
            gfx.DrawLine(linePen, x, y + rowHeight, x + width, y + rowHeight);
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
