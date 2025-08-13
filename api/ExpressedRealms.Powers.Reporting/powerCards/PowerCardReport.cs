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
        
        var secondaryColor = Color.FromARGB(125, 0, 0, 0);
        
        return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.DefaultTextStyle(x => x.FontSize(7.75f));

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
                                            .BorderColor(Color.FromARGB(10, 0, 0, 0))
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
                leftSide.Item().Repeat().Section(power.Name).Text(text =>
                {
                    text.Span(power.Name).Bold().FontSize(11).ExtraBold();

                    if (power.Category?.Count > 0)
                    {
                        text.Span($" - {string.Join(", ", power.Category)}").Italic().FontSize(11)
                            .FontColor(secondaryColor);
                    }
                });
                leftSide.Item().Text($"{power.ExpressionName} > {power.PathName}").Italic().FontSize(6)
                    .FontColor(secondaryColor);
                leftSide.FormatMainSection(null, power.Description);
                leftSide.FormatMainSection("Game Mechanic Effect", power.GameMechanicEffect);
                leftSide.FormatMainSection("Limitation", power.Limitation);
            });

            row.ConstantItem(1.75f, Unit.Inch).Column(rightSide =>
            {
                rightSide.FormatAttributeSection("Level", power.PowerLevel);
                rightSide.FormatAttributeSection("Power Use", power.IsPowerUse ? "Yes" : "No");
                rightSide.FormatAttributeSection("Cost", power.Cost);
                rightSide.FormatAttributeSection("Power Duration", power.PowerDuration);
                rightSide.FormatAttributeSection("Area of Effect", power.AreaOfEffect);
                rightSide.FormatAttributeSection("Activation Type", power.PowerActivationType);
                rightSide.FormatOtherSection(power.Other);
                rightSide.FormatPrerequisites(power.Prerequisites);
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
            .Replace("<strong>", "<b>")
            .Replace("</strong>", "</b> ");
        
        cell.Item().HTML(html =>
        {
            html.SetContainerStyleForHtmlElement("p", x => x.Padding(0));
            html.SetHtml(attributeValue);
        });
    }

    private static void FormatMainSection(this ColumnDescriptor cell, string? name, string? attributeValue)
    {

        if (attributeValue is null)
            return;

        var processedValue = attributeValue.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", "");
        if (string.IsNullOrEmpty(processedValue.Trim()))
            return;

        if (name is not null)
            cell.Item().Text($"{name}:").Bold();
        
        attributeValue = attributeValue
            .Replace("<strong>", "<b>")
            .Replace("</strong>", "</b>");
        
        cell.Item().PaddingBottom(0).HTML(html =>
        {
            html.SetContainerStyleForHtmlElement("p", x => x.Padding(0).PaddingBottom(2));
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
                text.SectionLink(prerequisites.PrerequisiteNames[0], prerequisites.PrerequisiteNames[0])
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
            
            cell.Item().Text(text =>
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
