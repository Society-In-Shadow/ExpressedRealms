using System.Runtime.InteropServices;
using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.PowerBookletFormat;

public static class PowerBookletReport
{
    public static Document GenerateReport(PowerBookletData data)
    {
        Settings.License = LicenseType.Community;

        return GetSingleTilePerPage(data);
    }

    private static Document GetSingleTilePerPage(PowerBookletData powerCards)
    {
        var secondaryColor = Color.FromARGB(125, 0, 0, 0);
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.DefaultTextStyle(x => x.FontSize(7.75f));
                page.Margin(0.75f, Unit.Inch);
                page.MarginTop(0.25f, Unit.Inch);
                page.MarginBottom(0.5f, Unit.Inch);

                page.Header()
                    .AlignCenter()
                    .PaddingBottom(10)
                    .Text($"{powerCards.ExpressionName} Power Booklet")
                    .FontSize(10)
                    .ExtraBold();

                page.Content()
                    .Column(col =>
                    {
                        col.Item()
                            .MultiColumn(multiColumn =>
                            {
                                multiColumn.Columns(3);
                                multiColumn.Spacing(10);

                                multiColumn
                                    .Content()
                                    .Column(columnContent =>
                                    {
                                        foreach (var powerPath in powerCards.PowerPaths)
                                        {
                                            columnContent
                                                .Item()
                                                .PaddingBottom(10)
                                                .Text(powerPath.Name)
                                                .FontSize(15)
                                                .ExtraBold();

                                            columnContent.FormatMainSection(
                                                null,
                                                powerPath.Description
                                            );

                                            columnContent.Item().PaddingBottom(10);

                                            powerPath.Powers = powerPath
                                                .Powers.OrderBy(x => x.PowerLevelId)
                                                .ThenBy(x => x.Name)
                                                .ToList();

                                            foreach (var power in powerPath.Powers)
                                            {
                                                FillPowerCard(columnContent, power, secondaryColor);
                                            }
                                        }
                                    });
                            });
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

    private static void FillPowerCard(ColumnDescriptor card, PowerData power, Color secondaryColor)
    {
        card.Item()
            .PaddingBottom(10)
            .Column(col =>
            {
                col.Item().Section(power.Name).Text(power.Name).Bold().FontSize(11).ExtraBold();

                col.Item()
                    .Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text(power.PowerLevel).Italic();
                        row.AutoItem()
                            .AlignRight()
                            .Text(string.Join(", ", power.Category))
                            .Italic();
                    });

                col.FormatMainSection(null, power.Description);
                col.FormatMainSection("Game Mechanic Effect", power.GameMechanicEffect);
                col.FormatMainSection("Limitation", power.Limitation);

                col.FormatAttributeSection("Power Use", power.IsPowerUse ? "Yes" : "No");
                col.FormatAttributeSection("Cost", power.Cost);
                col.FormatAttributeSection("Power Duration", power.PowerDuration);
                col.FormatAttributeSection("Area of Effect", power.AreaOfEffect);
                col.FormatAttributeSection("Activation Type", power.PowerActivationType);
                col.FormatOtherSection(power.Other);
                col.FormatPrerequisites(power.Prerequisites);
            });
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
