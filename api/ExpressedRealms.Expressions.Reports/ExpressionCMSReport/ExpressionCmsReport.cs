using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using QuestPDF;
using QuestPDF.Fluent;
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
                var header = $"{data.ExpressionName} Booklet";
                if (data.IsExpression)
                {
                    header = $"{data.ExpressionName} Background Booklet";
                }

                CommonElements.AddHeader(page, header);

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

                CommonElements.AddFooter(page);
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
            var groups = section.Blessings.GroupBy(x => x.Type).OrderBy(x => x.Key);
            foreach (var group in groups)
            {
                card.Item().PaddingBottom(10).Text(group.Key).FontSize(10).ExtraBold();

                foreach (var blessing in group.OrderBy(x => x.SubType).ThenBy(x => x.Name))
                {
                    card.Item()
                        .Row(row =>
                        {
                            row.RelativeItem().Text(blessing.Name).Bold().FontSize(9);
                            row.AutoItem().Text(blessing.SubType).Italic();
                        });
                    card.FormatMainSection(null, blessing.Description);
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
