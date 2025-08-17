using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards;

public static class PowerCardReport
{
    public static Document GenerateReport(List<PowerCardData> powerCards, bool isFiveByThree)
    {
        Settings.License = LicenseType.Community;

        powerCards = powerCards
            .OrderBy(x => x.PowerLevel)
            .ThenBy(x => x.PathName)
            .ThenBy(x => x.Name)
            .ToList();

        return GetSingleTilePerPage(powerCards, isFiveByThree);
    }

    public static MemoryStream GenerateSixUpPdf(List<PowerCardData> powerCards, bool isFiveByThree)
    {
        var singleTileDoc = GenerateReport(powerCards, isFiveByThree);
        using var srcStream = new MemoryStream();
        singleTileDoc.GeneratePdf(srcStream);
        var pdfBytes = srcStream.ToArray();

        // 2) Load the PDF once as an XPdfForm and use PageNumber to draw specific pages.
        using var outDoc = new PdfDocument();

        // US Letter Landscape in points (72 dpi): 11" x 8.5" => 792 x 612
        const double sheetWidth = 792.0;
        const double sheetHeight = 612.0;

        const int cols = 2;
        const int rows = 3;

        // Margins/gutters (points)
        const double outerMargin = 10;  // 0.25"
        const double gutterX = 10;      // between columns
        const double gutterY = 10;      // between rows

        double contentWidth = sheetWidth - (outerMargin * 2) - gutterX * (cols - 1);
        double contentHeight = sheetHeight - (outerMargin * 2) - gutterY * (rows - 1);
        double tileWidth = contentWidth / cols;
        double tileHeight = contentHeight / rows;

        // Load once and then switch PageNumber as we place pages
        using var form = XPdfForm.FromStream(new MemoryStream(pdfBytes));
        int totalPages = form.PageCount;

        int pageIndex = 0;
        while (pageIndex < totalPages)
        {
            var outPage = outDoc.AddPage();
            outPage.Orientation = PageOrientation.Landscape;
            outPage.Width = sheetWidth;
            outPage.Height = sheetHeight;

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

    private static Document GetSingleTilePerPage(List<PowerCardData> powerCards, bool isFiveByThree)
    {
        var secondaryColor = Color.FromARGB(125, 0, 0, 0);
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

                page.Content()
                    .Column(col =>
                    {
                        foreach (var power in powerCards)
                        {
                            FillPowerCard(col, power, secondaryColor);
                        }
                    });
            });
        });
    }

    private static void FillPowerCard(
        ColumnDescriptor card,
        PowerCardData power,
        Color secondaryColor
    )
    {
        card.Item()
            .Padding(15)
            .Decoration(decorator =>
            {
                decorator
                    .Before()
                    .Column(leftSide =>
                    {
                        leftSide
                            .Item()
                            .SkipOnce()
                            .Text(power.Name + " Continued")
                            .Bold()
                            .FontSize(11)
                            .ExtraBold();
                        leftSide
                            .Item()
                            .SkipOnce()
                            .Text($"{power.ExpressionName} > {power.PathName}")
                            .Italic()
                            .FontSize(6)
                            .FontColor(secondaryColor);
                    });

                decorator
                    .Content()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .PaddingRight(5)
                            .Column(leftSide =>
                            {
                                leftSide
                                    .Item()
                                    .Section(power.Name)
                                    .Text(text =>
                                    {
                                        text.Span(power.Name).Bold().FontSize(11).ExtraBold();

                                        if (power.Category?.Count > 0)
                                        {
                                            text.Span($" - {string.Join(", ", power.Category)}")
                                                .Italic()
                                                .FontSize(11)
                                                .FontColor(secondaryColor);
                                        }
                                    });
                                leftSide
                                    .Item()
                                    .Text($"{power.ExpressionName} > {power.PathName}")
                                    .Italic()
                                    .FontSize(6)
                                    .FontColor(secondaryColor);
                                leftSide.FormatMainSection(null, power.Description);
                                leftSide.FormatMainSection(
                                    "Game Mechanic Effect",
                                    power.GameMechanicEffect
                                );
                                leftSide.FormatMainSection("Limitation", power.Limitation);
                            });

                        row.ConstantItem(1.75f, Unit.Inch)
                            .Column(rightSide =>
                            {
                                rightSide.FormatAttributeSection("Level", power.PowerLevel);
                                rightSide.FormatAttributeSection(
                                    "Power Use",
                                    power.IsPowerUse ? "Yes" : "No"
                                );
                                rightSide.FormatAttributeSection("Cost", power.Cost);
                                rightSide.FormatAttributeSection(
                                    "Power Duration",
                                    power.PowerDuration
                                );
                                rightSide.FormatAttributeSection(
                                    "Area of Effect",
                                    power.AreaOfEffect
                                );
                                rightSide.FormatAttributeSection(
                                    "Activation Type",
                                    power.PowerActivationType
                                );
                                rightSide.FormatOtherSection(power.Other);
                                rightSide.FormatPrerequisites(power.Prerequisites);
                            });
                    });
            });
        card.Item().PageBreak();
    }

    private static void FormatAttributeSection(
        this ColumnDescriptor cell,
        string attributeName,
        string? attributeValue
    )
    {
        if (attributeValue is null)
            return;

        cell.Item()
            .Text(text =>
            {
                text.Span(attributeName).Bold();
                text.Span($": {attributeValue}");
            });
    }

    private static void FormatOtherSection(this ColumnDescriptor cell, string? attributeValue)
    {
        if (attributeValue is null)
            return;

        attributeValue = attributeValue.Replace("<strong>", "<b>").Replace("</strong>", "</b> ");

        cell.Item()
            .HTML(html =>
            {
                html.StandardHtmlFormatting();
                html.SetHtml(attributeValue);
            });
    }

    private static void FormatMainSection(
        this ColumnDescriptor cell,
        string? name,
        string? attributeValue
    )
    {
        if (attributeValue is null)
            return;

        var processedValue = attributeValue
            .Replace("<p>", "")
            .Replace("</p>", "")
            .Replace("&nbsp;", "");
        if (string.IsNullOrEmpty(processedValue.Trim()))
            return;

        if (name is not null)
            cell.Item().Text($"{name}:").Bold();

        attributeValue = HtmlTextFormatter.NormalizeParagraphsInsideTd(attributeValue);

        cell.Item()
            .PaddingBottom(0)
            .HTML(html =>
            {
                html.StandardHtmlFormatting();
                html.SetHtml(attributeValue);
            });
    }

    private static void FormatPrerequisites(
        this ColumnDescriptor cell,
        PrerequisiteData? prerequisites
    )
    {
        if (prerequisites is null)
            return;

        if (prerequisites.PrerequisiteNames.Count == 1)
        {
            cell.Item()
                .Text(text =>
                {
                    text.Span("Prerequisites: ").Bold();
                    text.SectionLink(
                            prerequisites.PrerequisiteNames[0],
                            prerequisites.PrerequisiteNames[0]
                        )
                        .Underline();
                });
        }
        else
        {
            var concatanator = "or";
            var anyAll = "Any";
            if (prerequisites.PrerequisiteNames.Count == prerequisites.Count)
            {
                anyAll = "All";
                concatanator = "and";
            }

            cell.Item()
                .Text(text =>
                {
                    text.Span("Prerequisites: ").Bold();
                    text.Span($"{anyAll} of the following powers: ");

                    var secondToLastIndex = prerequisites.PrerequisiteNames.Count - 2;
                    var last = prerequisites.PrerequisiteNames.Count - 1;
                    int i = 0;
                    foreach (var prerequisite in prerequisites.PrerequisiteNames)
                    {
                        text.SectionLink(prerequisite, prerequisite).Underline();

                        if (last == i)
                            return;
                        text.Span(secondToLastIndex == i ? $" {concatanator} " : ", ");
                        i++;
                    }
                });
        }
    }
}
