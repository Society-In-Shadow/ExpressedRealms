using ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;
using ExpressedRealms.Shared.Reports;
using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB.DataCards.AdDisadCards;

public class PopulateAdDisadCards : ICardTile
{
    private readonly AddDisadCardData _data;

    public PopulateAdDisadCards(AddDisadCardData data)
    {
        _data = data;
    }

    public void Populate(ColumnDescriptor col)
    {
        foreach (var blessing in _data.Blessings)
        {
            FillCard(col, blessing);
            col.Item().PageBreak();
        }
    }

    public static void FillCard(ColumnDescriptor card, BlessingInfo blessing)
    {
        var secondaryColor = Color.FromARGB(125, 0, 0, 0);
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
                            .Text(blessing.Name + " Continued")
                            .Bold()
                            .FontSize(11)
                            .ExtraBold();
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
                                    .Section(blessing.Name)
                                    .Text($"{blessing.Name} - {blessing.LevelName}")
                                    .Bold()
                                    .FontSize(11)
                                    .ExtraBold();

                                leftSide
                                    .Item()
                                    .Text(blessing.BlessingType)
                                    .Italic()
                                    .FontSize(6)
                                    .FontColor(secondaryColor);

                                FormatMainSection(leftSide, "Description", blessing.Description);
                                FormatMainSection(
                                    leftSide,
                                    "Level Effect",
                                    blessing.LevelDescription
                                );
                                if (!string.IsNullOrWhiteSpace(blessing.UserNotes))
                                    FormatMainSection(leftSide, "User Notes", blessing.UserNotes);
                            });
                    });
            });
    }

    private static void FormatMainSection(
        ColumnDescriptor cell,
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
            .PaddingBottom(0)
            .HTML(html =>
            {
                html.StandardHtmlFormatting();
                html.SetHtml(attributeValue);
            });
    }
}
