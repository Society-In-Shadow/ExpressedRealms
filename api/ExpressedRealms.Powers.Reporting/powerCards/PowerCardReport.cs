using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.CashCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.PowerCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards;

public static class PowerCardReport
{
    public static Document GenerateReport(List<DataCard> powerCards, bool isFiveByThree)
    {
        Settings.License = LicenseType.Community;

        powerCards = powerCards
            .OrderBy(x => x.CardType == CardType.PowerCard ? 0 : 1)
            .ThenBy(x => x.CardType == CardType.WealthCard ? 0 : 1)
            .ThenBy(x => x.CardType == CardType.CashCard ? 0 : 1)
            .ToList();

        return GetSingleTilePerPage(powerCards, isFiveByThree);
    }

    public static MemoryStream GenerateSixUpPdf(
        List<DataCard> powerCards,
        bool isFiveByThree,
        bool includeWealthCard = false
    )
    {
        var singleTileDoc = GenerateReport(powerCards, isFiveByThree);
        var srcStream = new MemoryStream();
        singleTileDoc.GeneratePdf(srcStream);

        if (isFiveByThree)
        {
            return srcStream;
        }

        var pdfBytes = srcStream.ToArray();
        srcStream.Dispose();

        // 2) Load the PDF once as an XPdfForm and use PageNumber to draw specific pages.
        using var outDoc = new PdfDocument();

        // US Letter Landscape in points (72 dpi): 11" x 8.5" => 792 x 612
        const double sheetWidth = 792.0;
        const double sheetHeight = 612.0;

        const int cols = 2;
        const int rows = 3;

        // Margins/gutters (points)
        const double outerMargin = 10; // 0.25"
        const double gutterX = 10; // between columns
        const double gutterY = 10; // between rows

        double contentWidth = sheetWidth - (outerMargin * 2) - gutterX * (cols - 1);
        double contentHeight = sheetHeight - (outerMargin * 2) - gutterY * (rows - 1);
        double tileWidth = contentWidth / cols;
        double tileHeight = contentHeight / rows;

        // Load once and then switch PageNumber as we place pages
        using var form = XPdfForm.FromStream(new MemoryStream(pdfBytes));
        int totalPages = form.PageCount - 1; // Convert from 1-based to 0-based

        int pageIndex = 0;
        while (pageIndex < totalPages)
        {
            var outPage = outDoc.AddPage();
            outPage.Orientation = PageOrientation.Landscape;
            outPage.Width = XUnit.FromPoint(sheetWidth);
            outPage.Height = XUnit.FromPoint(sheetHeight);

            using var gfx = XGraphics.FromPdfPage(outPage);

            // Draw light gray cut lines centered in the gutters between tiles
            var pen = new XPen(XColors.LightGray, 0.5);
            pen.DashStyle = XDashStyle.Dash;
            pen.DashPattern = new double[] { 15f, 15f };

            // Vertical lines (between columns)
            for (int c = 0; c < cols - 1; c++)
            {
                double x = outerMargin + (c + 1) * tileWidth + c * gutterX + gutterX / 2.0;
                double y1 = outerMargin;
                double y2 = outerMargin + contentHeight + (rows - 1) * gutterY;
                gfx.DrawLine(pen, x, y1, x, y2);
            }

            // Horizontal lines (between rows)
            for (int r = 0; r < rows - 1; r++)
            {
                double y = outerMargin + (r + 1) * tileHeight + r * gutterY + gutterY / 2.0;
                double x1 = outerMargin;
                double x2 = outerMargin + contentWidth + (cols - 1) * gutterX;
                gfx.DrawLine(pen, x1, y, x2, y);
            }

            for (int r = 0; r < rows && pageIndex < totalPages; r++)
            {
                for (int c = 0; c < cols && pageIndex < totalPages; c++)
                {
                    form.PageNumber = pageIndex + 1; // 1-based
                    double x = outerMargin + c * (tileWidth + gutterX);
                    double y = outerMargin + r * (tileHeight + gutterY);
                    gfx.DrawImage(form, x, y, tileWidth, tileHeight);
                    pageIndex++;
                }
            }
        }

        var outStream = new MemoryStream();
        outDoc.Save(outStream, false);
        return outStream;
    }

    private static Document GetSingleTilePerPage(List<DataCard> powerCards, bool isFiveByThree)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                if (isFiveByThree)
                {
                    page.Size(5, 3, Unit.Inch);
                }
                else
                {
                    page.Size(
                        PageSizes.Letter.Landscape().Width / 2,
                        PageSizes.Letter.Landscape().Height / 3
                    );
                }

                page.DefaultTextStyle(x => x.FontSize(7.75f));
                page.Background()
                    .Column(col =>
                    {
                        var backgroundTextColor = Color.FromARGB(45, 0, 0, 0);
                        col.Item()
                            .PaddingTop(0.06f, Unit.Inch)
                            .PaddingRight(0.25f, Unit.Inch)
                            .Text($"Society in Shadows - {DateTime.Now.ToString("MMMM/d/yyyy")}")
                            .FontColor(backgroundTextColor)
                            .AlignEnd();
                    });
                page.Content()
                    .Column(col =>
                    {
                        foreach (var card in powerCards)
                        {
                            switch (card.CardType)
                            {
                                case CardType.PowerCard:
                                    PopulatePowerCard.FillCard(col, (PowerCardData)card.CardData);
                                    break;
                                case CardType.WealthCard:
                                    PopulateWealthCard.FillCard(col, (WealthCardData)card.CardData);
                                    break;
                                case CardType.CashCard:
                                    PopulateCashCard.FillCard(col, (CashCardData)card.CardData);
                                    break;
                            }
                        }
                    });
            });
        });
    }
}
