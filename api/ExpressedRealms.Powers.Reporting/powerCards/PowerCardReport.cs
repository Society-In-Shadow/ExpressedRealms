using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards;

public static class PowerCardReport
{
    public static Document GenerateReport(List<PowerCardData> powerCards)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        var chunks = powerCards.Chunk(6);
        
        var secondaryColor = Color.FromARGB(100, 0, 0, 0);
        
        return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.DefaultTextStyle(x => x.FontSize(7f));

                    float rowHeight = (PageSizes.Letter.Landscape().Height / 3);

                    page.Content().Column(col =>
                    {
                        foreach (var chunk in chunks)
                        {
                            for (int pageRow = 0; pageRow < 3; pageRow++)
                            {
                                col.Item().Height(rowHeight).Row(row =>
                                {
                                    for (int f = pageRow * 2; f < pageRow * 2 + 2; f++)
                                    {
                                        if (chunk.Length < f + 1)
                                            break;
                                    
                                        var power = chunk[f];
                                    
                                        row.RelativeItem()
                                            .Border(1)
                                            .Column(cell =>
                                            {
                                                FillPowerCard(cell, power, secondaryColor);
                                            });
                                    }
                                });
                            }
                        }
                    });
                });
        });
    }

    private static void FillPowerCard(ColumnDescriptor card, PowerCardData power, Color secondaryColor)
    {
        card.Item().Padding(15).Row(row =>
        {
            row.RelativeItem().PaddingRight(5).Column(leftSide =>
            {
                leftSide.Item().Text(text =>
                {
                    text.Span(power.Name).Bold().FontSize(11).ExtraBold();

                    if (power.Category?.Count > 0)
                    {
                        text.Span($" - {string.Join(", ", power.Category)}").Italic().FontSize(10)
                            .FontColor(secondaryColor);
                    }
                });
                leftSide.Item().Text($"{power.ExpressionName} > {power.PathName}").Italic().FontSize(6)
                    .FontColor(secondaryColor);
                leftSide.Item().Text(power.Description);
                leftSide.Item().Text("Game Mechanic Effect:").Bold();
                leftSide.Item().Text(power.GameMechanicEffect);

                if (power.Limitation is not null)
                {
                    leftSide.Item().Text("Limitation:").Bold();
                    leftSide.Item().Text(power.Limitation);
                }
            });

            row.ConstantItem(1.75f, Unit.Inch).Column(rightSide =>
            {
                rightSide.FormatAttributeSection("Categories", string.Join(", ", power.Category ?? new List<string>()));
                rightSide.FormatAttributeSection("Level", power.PowerLevel);
                rightSide.FormatAttributeSection("Power Use", power.IsPowerUse ? "Yes" : "No");
                rightSide.FormatAttributeSection("Cost", power.Cost);
                rightSide.FormatAttributeSection("Power Duration", power.PowerDuration);
                rightSide.FormatAttributeSection("Area of Effect", power.AreaOfEffect);
                rightSide.FormatAttributeSection("Power Duration", power.PowerDuration);
                rightSide.FormatAttributeSection("Activation Type", power.PowerActivationType);
                rightSide.FormatPrerequisites(power.Prerequisites);
                rightSide.FormatOtherSection(power.Other);
            });
        });

    }
    
    private static void FormatAttributeSection(this ColumnDescriptor cell, string attributeName, string? attributeValue)
    {
        if (attributeValue is null)
            return;
        
        cell.Item().Text(text =>
        {
            text.Span(attributeName).Bold();
            text.Span($": {attributeValue}");
        });
    }
    
    private static void FormatOtherSection(this ColumnDescriptor cell, string? attributeValue)
    {
        if (attributeValue is null)
            return;
        
        attributeValue = attributeValue
            .Replace("<p>", "")
            .Replace("</p>", "<br/>")
            .Replace("<strong>", "<b>")
            .Replace("</strong>", "</b>");
        
        cell.Item().HTML(html =>
        {
            html.SetHtml(attributeValue);
        });
    }

    private static void FormatPrerequisites(this ColumnDescriptor cell, PrerequisiteData? prerequisites)
    {
        if (prerequisites is null)
            return;


        if (prerequisites.PrerequisiteNames.Count == 1)
        {
            cell.Item().Text(text =>
            {
                text.Span("Prerequisites: ").Bold();
                text.Span(prerequisites.PrerequisiteNames[0]);
            });
        }
        else if (prerequisites.PrerequisiteNames.Count == prerequisites.Count)
        {
            cell.Item().Text(text =>
            {
                text.Span("Prerequisites: ").Bold();
                text.Span("All of the following powers:" + string.Join(", ", prerequisites.PrerequisiteNames));
            });
        }
        else if (prerequisites.PrerequisiteNames.Count <= prerequisites.Count)
        {
            cell.Item().Text(text =>
            {
                text.Span("Prerequisites: ").Bold();
                text.Span("Any of the following powers: " + string.Join(", ", prerequisites.PrerequisiteNames));
            });
        }
    }
}
