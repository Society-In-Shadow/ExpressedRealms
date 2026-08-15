using QuestPDF.Elements.Table;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB.DataCards.WealthCards;

internal static class Utilities
{
    public static void AddFormattedCell(
        this ITableCellContainer initialIncomeRow,
        string stampText,
        bool isBold = false
    )
    {
        var text = initialIncomeRow
            .Border(1)
            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
            .Padding(3)
            .AlignCenter()
            .AlignMiddle()
            .Text(stampText);

        if (isBold)
            text.ExtraBold();
    }
    
    public static void AddFormattedHeaderCell(
        this ITableCellContainer initialIncomeRow,
        string stampText
    )
    {
        initialIncomeRow
            .Border(1)
            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
            .PaddingTop(3)
            .PaddingBottom(3)
            .AlignCenter()
            .AlignMiddle()
            .Text(stampText)
            .Bold();
    }

    public static void CreateStamp(this IContainer initialIncomeRow, string stampText)
    {
        initialIncomeRow
            .Width(0.67f, Unit.Inch)
            .Height(0.67f, Unit.Inch)
            .Border(1)
            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
            .AlignCenter()
            .AlignMiddle()
            .Text(stampText)
            .FontSize(8);
    }

    public static void CheckboxItem(this IContainer container, string stampText, string label)
    {
        container
            .AlignMiddle()
            .Row(inner =>
            {
                inner.Spacing(3);

                inner
                    .ConstantItem(0.11f, Unit.Inch)
                    .Height(0.11f, Unit.Inch)
                    .Border(1)
                    .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(stampText)
                    .FontSize(6);

                inner.AutoItem().AlignMiddle().PaddingTop(-1).Text(label);
            });
    }
}