using QuestPDF.Elements.Table;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;

internal static class PopulateWealthCard
{
    public static void FillCard(ColumnDescriptor card, WealthCardData wealthData)
    {
        card.Item()
            .PaddingTop(15)
            .PaddingLeft(15)
            .PaddingRight(15)
            .PaddingRight(7)
            .Decoration(decorator =>
            {
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
                                    .Row(initialIncomeRow =>
                                    {
                                        initialIncomeRow
                                            .RelativeItem()
                                            .Text(text =>
                                            {
                                                text.Span("Wealth Card")
                                                    .Bold()
                                                    .FontSize(11)
                                                    .ExtraBold();

                                                text.Span($" - {wealthData.CharacterName}")
                                                    .Italic()
                                                    .FontSize(11)
                                                    .FontColor(CustomColors.SecondaryTextColor);
                                            });

                                        initialIncomeRow
                                            .RelativeItem()
                                            .AlignRight()
                                            .Text("Wealth Level: " + wealthData.WealthLevel)
                                            .Bold()
                                            .FontSize(11)
                                            .ExtraBold();
                                    });

                                leftSide
                                    .Item()
                                    .PaddingBottom(5)
                                    .Row(topRow =>
                                    {
                                        topRow
                                            .RelativeItem()
                                            .PaddingTop(5)
                                            .Column(descriptions =>
                                            {
                                                descriptions
                                                    .Item()
                                                    .Text(
                                                        "Warning: Do not throw this card away, this is part of your character sheet"
                                                    );
                                                descriptions
                                                    .Item()
                                                    .Text(
                                                        "Level Increase / Liquidations are story driven - you need a GO to do either"
                                                    );

                                                FillBlessings(wealthData, descriptions);
                                            });

                                        topRow
                                            .ConstantItem(1.18f, Unit.Inch)
                                            .PaddingTop(5)
                                            .AlignMiddle()
                                            .AlignCenter()
                                            .CreateStamp(
                                                $"Starting Income \n${wealthData.InitialBasicItemIncome:N0}"
                                            );
                                    });

                                leftSide
                                    .Item()
                                    .Row(initialIncomeRow =>
                                    {
                                        GenerateWealthTableAndStamps(wealthData, initialIncomeRow);
                                    });
                            });
                    });
            });
        card.Item().PageBreak();
    }

    private static void FillBlessings(WealthCardData wealthData, ColumnDescriptor descriptions)
    {
        descriptions
            .Item()
            .PaddingTop(12)
            .Row(blessingRow =>
            {
                blessingRow.Spacing(10);

                var hasDestitute = wealthData.AppliedBlessings.Any(x => x.Key == "Destitute");
                var hasDisowned = wealthData.AppliedBlessings.Any(x =>
                    x.Key == "Disowned / Disfavored"
                );
                var hasWealthy = wealthData.AppliedBlessings.Any(x => x.Key == "Wealthy");

                blessingRow.AutoItem().CheckboxItem(hasDestitute ? "X" : "", "Destitute");
                blessingRow
                    .AutoItem()
                    .CheckboxItem(hasDisowned ? "X" : "", "Disowned / Disfavored");
                blessingRow.AutoItem().CheckboxItem(hasWealthy ? "X" : "", "Wealthy");
            });
    }

    private static void GenerateWealthTableAndStamps(
        WealthCardData wealthData,
        RowDescriptor levelTableRow
    )
    {
        // Left Side Stamp Boxes
        levelTableRow
            .ConstantItem(1.45f, Unit.Inch)
            .AlignMiddle()
            .AlignCenter()
            .Column(initialIncomeColumn =>
            {
                initialIncomeColumn
                    .Item()
                    .PaddingBottom(5)
                    .Row(levelIncreaseRow =>
                    {
                        levelIncreaseRow.RelativeItem().CreateStamp("Level Increase");
                        levelIncreaseRow.RelativeItem().CreateStamp("Level Increase");
                    });

                initialIncomeColumn
                    .Item()
                    .Row(levelIncreaseRow =>
                    {
                        levelIncreaseRow.RelativeItem().CreateStamp("Level Liquidated");
                        levelIncreaseRow.RelativeItem().CreateStamp("Level Liquidated");
                    });
            });

        levelTableRow
            .RelativeItem()
            .AlignRight()
            .AlignCenter()
            .AlignMiddle()
            .Table(wealthTable =>
            {
                wealthTable.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(0.41f, Unit.Inch);
                    columns.ConstantColumn(1.01f, Unit.Inch);
                    columns.ConstantColumn(0.861f, Unit.Inch);
                    columns.ConstantColumn(1.07f, Unit.Inch);
                });

                wealthTable.Header(header =>
                {
                    header.Cell().AddFormattedHeaderCell("Level");
                    header.Cell().AddFormattedHeaderCell("Session Income");
                    header.Cell().AddFormattedHeaderCell("Cash to Level Up");
                    header.Cell().AddFormattedHeaderCell("Liquidation Value");
                });

                int i = 1;
                foreach (var level in wealthData.WealthTableLines)
                {
                    var levelNumber = level.Level == -1 ? "N/A" : level.Level.ToString();
                    var levelIncome =
                        Math.Abs(level.Income - (-1)) < 1 ? "N/A" : $"${level.Income.ToString("N0")}";
                    var levelCash =
                        Math.Abs(level.CashToLevelUp - (-1)) < 1
                            ? "N/A"
                            : $"${level.CashToLevelUp.ToString("N0")}";
                    var levelLiquidation =
                        Math.Abs(level.LiquidationAmount - (-1)) < 1
                            ? "N/A"
                            : $"${level.LiquidationAmount.ToString("N0")}";

                    wealthTable.Cell().AddFormattedCell(levelNumber, i == 3);
                    wealthTable.Cell().AddFormattedCell(levelIncome, i == 3);
                    wealthTable.Cell().AddFormattedCell(levelCash, i == 3);
                    wealthTable.Cell().AddFormattedCell(levelLiquidation, i == 3);
                    i++;
                }
            });
    }

    private static void AddFormattedCell(
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

    private static void AddFormattedHeaderCell(
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

    private static void CreateStamp(this IContainer initialIncomeRow, string stampText)
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

    private static void CheckboxItem(this IContainer container, string stampText, string label)
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
