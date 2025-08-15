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

        data.Sections = data.Sections
            .OrderBy(x => x.SortOrder)
            .ToList();

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

                page.Content()
                    .Column(col =>
                    {
                        foreach (var power in data.Sections)
                        {
                            col.FillSection(power);
                        }
                    });
            });
        });
    }

    private static void FillSection(
        this ColumnDescriptor card,
        SectionData section
    )
    {
        card.Item().Text(section.Name)
                            .Bold()
                            .FontSize(11)
                            .ExtraBold();

        card.FormatMainSection(null, section.Content);
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

        attributeValue = attributeValue.Replace("<p>", "").Replace("</p>", "\n");
        attributeValue = attributeValue.Replace("<strong>", "<b>").Replace("</strong>", "</b>");

        cell.Item()
            .PaddingBottom(0)
            .HTML(html =>
            {
                html.SetHtml(attributeValue);
            });
    }
}
