using System.ComponentModel;
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
                    text.Span(power.Name).Bold().FontSize(9);

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
                rightSide.FormatAttributeSection("Level", power.PowerLevel);
                rightSide.FormatAttributeSection("Power Use", power.IsPowerUse ? "Yes" : "No");
                rightSide.FormatAttributeSection("Cost", power.Cost);
                rightSide.FormatAttributeSection("Power Duration", power.PowerDuration);
                rightSide.FormatAttributeSection("Area of Effect", power.AreaOfEffect);
                rightSide.FormatAttributeSection("Power Duration", power.PowerDuration);
                rightSide.FormatAttributeSection("Activation Type", power.PowerActivationType);
                
                rightSide.Item().Text(power.PrerequisitesNeeded);
                rightSide.Item().Text(power.Prerequisites?.PrerequisiteNames.Count ?? 0);
            });
        });

    }
    
    private static void FormatAttributeSection(this ColumnDescriptor cell, string attributeName, string attributeValue)
    {
        cell.Item().Text(text =>
        {
            text.Span(attributeName).Bold();
            text.Span($": {attributeValue}");
        });
    }
}
