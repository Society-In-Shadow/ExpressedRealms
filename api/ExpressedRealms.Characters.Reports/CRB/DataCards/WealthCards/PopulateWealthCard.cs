using ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB.DataCards.WealthCards;

public class PopulateWealthCard : ICardTile
{
    private readonly WealthCardData _data;

    public PopulateWealthCard(WealthCardData data)
    {
        _data = data;
    }

    public void Populate(ColumnDescriptor col)
    {
        col.Item()
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
                                            .Text("Wealth Card")
                                            .Bold()
                                            .FontSize(11)
                                            .ExtraBold();

                                        initialIncomeRow
                                            .RelativeItem()
                                            .AlignRight()
                                            .Text("Wealth Level: " + _data.WealthLevel)
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

                                                FillBlessings(_data, descriptions);
                                            });

                                        topRow
                                            .ConstantItem(1.18f, Unit.Inch)
                                            .PaddingTop(5)
                                            .AlignMiddle()
                                            .AlignCenter()
                                            .CreateStamp(
                                                $"Starting Income \n${_data.InitialBasicItemIncome:N0}"
                                            );
                                    });

                                leftSide
                                    .Item()
                                    .Row(initialIncomeRow =>
                                    {
                                        GenerateWealthTableAndStamps(_data, initialIncomeRow);
                                    });
                            });
                    });
            });
        col.Item().PageBreak();
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
                        Math.Abs(level.Income - (-1)) < 1
                            ? "N/A"
                            : $"${level.Income.ToString("N0")}";
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
}
