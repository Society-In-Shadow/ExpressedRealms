using ExpressedRealms.Powers.Reporting.PowerBookletFormat;
using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Expressions.Reports.FactionReport;

public static class FactionReport
{
    public static Document GenerateReport(FactionReportData data)
    {
        Settings.License = LicenseType.Community;

        data.Factions = data.Factions.OrderBy(x => x.Name).ToList();

        return GetSingleTilePerPage(data);
    }

    private static Document GetSingleTilePerPage(FactionReportData data)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                var header = $"{data.ExpressionName} Faction Booklet";

                CommonElements.AddHeader(page, header);

                page.Content()
                    .Column(col =>
                    {
                        col.Item()
                            .MultiColumn(mCol =>
                            {
                                mCol.Spacing(10);
                                mCol.Content()
                                    .Column(columnContent =>
                                    {
                                        foreach (var power in data.Factions)
                                        {
                                            columnContent.FillSection(power);
                                        }
                                    });
                            });
                    });

                CommonElements.AddFooter(page);
            });
        });
    }
    
    private static string GetArticle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "A";

        return "AEIOUaeiou".Contains(value[0]) ? "An" : "A";
    }
    
    private static void FillSection(this ColumnDescriptor card, FactionData section)
    {
        card.Item()
            .PaddingBottom(10)
            .PaddingTop(10)
            .Text(section.Name)
            .Bold()
            .FontSize(11)
            .ExtraBold();

        card.FormatMainSection(null, section.Background);

        foreach (var level in section.FactionLevels)
        {
            card.Item().Text($"{level.RankName} Rank").Bold().FontSize(11);

            card.Item().PaddingTop(10).Text($"Requirements:").Bold();

            if (string.IsNullOrWhiteSpace(level.KnowledgeName))
            {
                card.Item().Text("No Requirements to Join.");
            }
            else
            {
                card.Item()
                    .Text(
                        $" - {GetArticle(level.KnowledgeLevel!)} \"{level.KnowledgeLevel}\" level in the {level.KnowledgeName} knowledge."
                    );
                card.Item()
                    .Text(
                        $" - Specialization in {level.KnowledgeSpecialization} for above knowledge."
                    );
                card.Item()
                    .Text(" - GO approval with the completion of one or more tasks / trails.");
            }

            card.Item().PaddingTop(10);
            if (level.Power is not null)
            {
                PowerBookletReport.FillPowerCard(card, level.Power, false);
            }
            else
            {
                card.Item().PaddingBottom(10).Text("No Known Powers for this Rank.");
            }
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
