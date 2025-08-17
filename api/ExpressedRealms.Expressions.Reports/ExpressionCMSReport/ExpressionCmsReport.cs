using System.Runtime.InteropServices;
using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Expressions.Reports.ExpressionCMSReport;

public static class ExpressionCmsReport
{
    public static Document GenerateReport(ExpressionCmsReportData data)
    {
        Settings.License = LicenseType.Community;

        data.Sections = data.Sections.OrderBy(x => x.SortOrder).ToList();

        return GetSingleTilePerPage(data);
    }

    private static Document GetSingleTilePerPage(ExpressionCmsReportData data)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.DefaultTextStyle(x => x.FontSize(7.75f));
                page.Margin(0.5f, Unit.Inch);
                page.MarginTop(0.25f, Unit.Inch);

                var header = $"{data.ExpressionName} Booklet";
                if (data.IsExpression)
                {
                    header = $"{data.ExpressionName} Background Booklet";
                }

                page.Header().AlignCenter().PaddingBottom(10).Text(header).FontSize(10).ExtraBold();

                page.Content()
                    .Column(col =>
                    {
                        // Temporary bypass to make sure equipment tables don't look weird
                        if (data.ExpressionName == "Inventory")
                        {
                            foreach (var power in data.Sections)
                            {
                                col.FillSection(power);
                            }
                        }
                        else
                        {
                            col.Item()
                                .MultiColumn(mCol =>
                                {
                                    mCol.Spacing(10);
                                    mCol.Content()
                                        .Column(columnContent =>
                                        {
                                            foreach (var power in data.Sections)
                                            {
                                                columnContent.FillSection(power);
                                            }
                                        });
                                });
                        }
                    });

                page.Footer()
                    .Row(row =>
                    {
                        var tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                            ? "Central Standard Time" // Windows ID
                            : "America/Chicago"; // IANA ID

                        var central = TimeZoneInfo.FindSystemTimeZoneById(tzId);
                        DateTimeOffset nowCentral = TimeZoneInfo.ConvertTime(
                            DateTimeOffset.UtcNow,
                            central
                        );

                        row.RelativeItem()
                            .Column(x =>
                            {
                                x.Item().Text($"©{nowCentral.Year} Expressed Realms");
                                x.Item().Text("Society in Shadows LARP. All rights reserved.");
                            });

                        row.RelativeItem()
                            .AlignCenter()
                            .Column(x =>
                            {
                                x.Item().Text("");
                                x.Item()
                                    .Text(text =>
                                    {
                                        text.Span("Page ");
                                        text.CurrentPageNumber();
                                        text.Span(" / ");
                                        text.TotalPages();
                                    });
                            });
                        row.RelativeItem()
                            .AlignRight()
                            .Column(x =>
                            {
                                x.Item().Text("");
                                x.Item().Text($"Download Date: {nowCentral:M/d/yyyy h:mm tt} CST");
                            });
                    });
            });
        });
    }

    private static void FillSection(this ColumnDescriptor card, SectionData section)
    {
        card.Item()
            .PaddingBottom(10)
            .PaddingTop(10)
            .Text(section.Name)
            .Bold()
            .FontSize(11)
            .ExtraBold();

        if (section.Knowledges is not null)
        {
            foreach (var knowledge in section.Knowledges)
            {
                card.Item()
                    .Row(row =>
                    {
                        row.RelativeItem().Text(knowledge.Name).Bold().FontSize(9);
                        row.AutoItem().Text(knowledge.Type).Italic();
                    });
                card.Item().PaddingBottom(10).Text(knowledge.Description);
            }
        }
        else if (section.Blessings is not null)
        {
            var groups = section.Blessings.GroupBy(x => x.Type);
            foreach (var group in groups)
            {
                card.Item().PaddingBottom(10).Text(group.Key).FontSize(10).ExtraBold();

                foreach (var blessing in group)
                {
                    card.Item()
                        .Row(row =>
                        {
                            row.RelativeItem().Text(blessing.Name).Bold().FontSize(9);
                            row.AutoItem().Text(blessing.SubType).Italic();
                        });
                    card.Item().Text(blessing.Description);
                    foreach (var level in blessing.Levels.OrderBy(x => x.Level))
                    {
                        card.Item().Text($" - {level.Level} - {level.Description}");
                    }
                    card.Item().PaddingBottom(10);
                }
            }
        }
        else
        {
            card.FormatMainSection(null, section.Content);
        }
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
        attributeValue = attributeValue.Replace("<strong>", "<b>").Replace("</strong>", "</b>");

        cell.Item()
            .PaddingBottom(10)
            .HTML(html =>
            {
                html.StandardHtmlFormatting();
                html.SetHtml(attributeValue);
            });
    }

}
