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
                                        initialIncomeRow.RelativeItem().Text(text =>
                                        {
                                            text.Span("Wealth Card").Bold().FontSize(11).ExtraBold();

                                                text.Span($" - asdf")
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
                                    .PaddingTop(5)
                                    .Text(
                                        "Warning: Do not throw this card away, this is part of your character sheet"
                                    );
                                leftSide
                                    .Item()
                                    .Text(
                                        "Level Increase / Liquidations are story driven - you need a GO to do either"
                                    );
                                
                                leftSide
                                    .Item()
                                    .Row(initialIncomeRow =>
                                    {
                                        
                                        // Left Side Stamp Boxes
                                        initialIncomeRow
                                            .ConstantItem(1.45f, Unit.Inch)
                                            .AlignMiddle()
                                            .PaddingBottom(3)
                                            .Column(initialIncomeColumn =>
                                            {
                                                initialIncomeColumn.Item().Row(levelIncreaseRow =>
                                                {
                                                    levelIncreaseRow.RelativeItem().PaddingBottom(5).CreateStamp("Level Increase");
                                                    levelIncreaseRow.RelativeItem().CreateStamp("Level Increase");
                                                });
                                                
                                                initialIncomeColumn.Item().Row(levelIncreaseRow =>
                                                {
                                                    levelIncreaseRow.RelativeItem().PaddingBottom(5).CreateStamp("Level Liquidated");
                                                    levelIncreaseRow.RelativeItem().CreateStamp("Level Liquidated");
                                                });
                                            });
                                        
                                        // Right Side Info
                                        initialIncomeRow
                                            .RelativeItem()
                                            .Column(tableRow =>
                                            {
                                                tableRow.Item().Row(rightTopRow =>
                                                {
                                                    rightTopRow.RelativeItem().Text("Modifiers");
                                                    
                                                    rightTopRow.RelativeItem()
                                                        .PaddingTop(5)
                                                        .AlignRight()
                                                    .Row(initialIncomeRow =>
                                                    {
                                                        initialIncomeRow
                                                            .RelativeItem()
                                                            .AlignMiddle()
                                                            .Column(initialIncomeColumn =>
                                                            {
                                                                initialIncomeColumn
                                                                    .Item()
                                                                    .Text("Initial Basic Income")
                                                                    .Bold()
                                                                    .AlignCenter()
                                                                    .FontSize(8);
                                                                initialIncomeColumn
                                                                    .Item()
                                                                    .Text(
                                                                        wealthData.InitialBasicItemIncome.ToString(
                                                                            "C0"
                                                                        )
                                                                    )
                                                                    .Bold()
                                                                    .AlignCenter()
                                                                    .FontSize(11);
                                                            });
                                                        initialIncomeRow.RelativeItem().AlignRight().CreateStamp("Spent Session Income");
                                                    });
                                                });
                                                
                                                tableRow.Item().Row(levelTableRow =>
                                                {
                                                    levelTableRow.RelativeItem().AlignRight().AlignBottom().Table(wealthTable =>
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
                                                        foreach (var level  in wealthData.WealthTableLines)
                                                        {
                                                            var levelNumber = level.Level == -1 ? "N/A" : level.Level.ToString();
                                                            var levelIncome = Math.Abs(level.Income - (-1)) < 0 ? "N/A" : level.Income.ToString("C0");
                                                            var levelCash = Math.Abs(level.CashToLevelUp - (-1)) < 0 ? "N/A" : level.CashToLevelUp.ToString("C0");
                                                            var levelLiquidation = Math.Abs(level.LiquidationAmount - (-1)) < 0 ? "N/A" : level.LiquidationAmount.ToString("C0");
                                                            
                                                            wealthTable.Cell().AddFormattedCell(levelNumber, i == 3);
                                                            wealthTable.Cell().AddFormattedCell(levelIncome, i == 3);
                                                            wealthTable.Cell().AddFormattedCell(levelCash, i == 3);
                                                            wealthTable.Cell().AddFormattedCell(levelLiquidation, i == 3);
                                                            i++;
                                                        }
                                                    });
                                                });
                                            });

                                    });
                            });
                    });
            });
        card.Item().PageBreak();
    }
    private static void AddFormattedCell(this ITableCellContainer initialIncomeRow, string stampText, bool isBold = false)
    {
        var text = initialIncomeRow
            .Border(1)
            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
            .Padding(3)

            .AlignCenter()
            .AlignMiddle()
            .Text(stampText);
        
        if(isBold)
            text.ExtraBold();
    }
    
    private static void AddFormattedHeaderCell(this ITableCellContainer initialIncomeRow, string stampText)
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
}
