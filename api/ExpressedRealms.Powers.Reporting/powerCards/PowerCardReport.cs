using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards;

public static class PowerCardReport
{
    public static Document GenerateReport(List<PowerCardData> powerCards, bool isFiveByThree)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return GetSingleTilePerPage(powerCards, isFiveByThree);;
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
                        ;
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
                html.SetContainerStyleForHtmlElement("p", x => x.Padding(0));
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

        attributeValue = attributeValue.Replace("<strong>", "<b>").Replace("</strong>", "</b>");

        cell.Item()
            .PaddingBottom(0)
            .HTML(html =>
            {
                html.SetContainerStyleForHtmlElement("p", x => x.Padding(0).PaddingBottom(2));
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
