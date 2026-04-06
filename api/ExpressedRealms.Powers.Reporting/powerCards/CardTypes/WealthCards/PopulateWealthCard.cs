using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;

internal static class PopulateWealthCard
{
    public static void FillCard(
        ColumnDescriptor card,
        WealthCardData wealthData
    )
    {
        
        card.Item()
            .Padding(15)
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
                                leftSide.Item().Row(initialIncomeRow =>
                                {
                                    initialIncomeRow.RelativeItem()
                                        .Text("Wealth Card").Bold().FontSize(11).ExtraBold();
                                    initialIncomeRow.RelativeItem()
                                        .AlignRight()
                                        .Text("Wealth Level: " + wealthData.WealthLevel).Bold().FontSize(11)
                                        .ExtraBold();
                                });
                                
                                leftSide.Item().PaddingTop(5).Text("Warning: Do not throw this card away after spending everything");
                                
                                leftSide.Item().PaddingTop(5).Row(initialIncomeRow =>
                                {
                                    initialIncomeRow.RelativeItem()
                                        .AlignMiddle()
                                        .Column(initialIncomeColumn =>
                                        {
                                            initialIncomeColumn.Item()
                                                .Text("Con Income")
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(8);
                                            initialIncomeColumn.Item()
                                                .Text(wealthData.WealthIncome.ToString("C0"))
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(11);
                                        });
                                    
                                    initialIncomeRow.RelativeItem()
                                        .AlignMiddle()
                                        .Column(initialIncomeColumn =>
                                        {
                                            initialIncomeColumn.Item()
                                                .Text("Banked Cash")
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(8);
                                            initialIncomeColumn.Item()
                                                .Text(wealthData.BankedCash.ToString("C0"))
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(11);
                                        });
                                    
                                    initialIncomeRow.RelativeItem()
                                        .AlignMiddle()
                                        .Column(initialIncomeColumn =>
                                        {
                                            initialIncomeColumn.Item()
                                                .Text("Liquidation Value")
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(8);
                                            initialIncomeColumn.Item()
                                                .Text(wealthData.Liquadation.ToString("C0"))
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(11);
                                        });
                                });
                                foreach (var rowCount in Enumerable.Range(1, 2))
                                {
                                    leftSide.Item()
                                        .PaddingTop(10).Row(costRow =>
                                    {
                                        costRow.ConstantItem(0.67f, Unit.Inch)
                                            .Width(0.67f, Unit.Inch)
                                            .Height(0.67f, Unit.Inch)
                                            .Border(1)
                                            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)]);

                                        costRow.RelativeItem()
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(20)
                                            .BorderTop(1)
                                            .BorderBottom(1);
                                        
                                        costRow.ConstantItem(0.67f, Unit.Inch)
                                            .Width(0.67f, Unit.Inch)
                                            .Height(0.67f, Unit.Inch)
                                            .Border(1)
                                            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)]);
                                        
                                        costRow.RelativeItem()
                                            .PaddingLeft(5)
                                            .PaddingTop(20)
                                            .BorderTop(1)
                                            .BorderBottom(1);
                                    });
                                }
                            });

                        row.ConstantItem(1.75f, Unit.Inch)
                            .Column(rightSide =>
                            {
                                rightSide.Item().Row(initialIncomeRow =>
                                {
                                    initialIncomeRow.ConstantItem(1.08f, Unit.Inch)
                                        .AlignMiddle()
                                        .Column(initialIncomeColumn =>
                                        {
                                            initialIncomeColumn.Item()
                                                .Text("Initial Basic Income")
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(8);
                                            initialIncomeColumn.Item()
                                                .Text(wealthData.InitialBasicItemIncome.ToString("C0"))
                                                .Bold()
                                                .AlignCenter()
                                                .FontSize(11);
                                        });
                                    initialIncomeRow.RelativeItem()
                                        .Width(0.67f, Unit.Inch)
                                        .Height(0.67f, Unit.Inch)
                                        .Border(1)
                                        .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text("Spent Initial Income")
                                        .FontSize(8);
                                });
                                
                                foreach (var rowCount in Enumerable.Range(1, 5))
                                {
                                    rightSide.Item()
                                        .PaddingTop(5).Row(costRow =>
                                        {
                                            costRow.ConstantItem(0.6f, Unit.Inch)
                                                .Height(0.25f, Unit.Inch)
                                                .BorderBottom(1);
                                        
                                            costRow.RelativeItem()
                                                .PaddingLeft(5)
                                                .BorderBottom(1);
                                        });
                                }
                            });
                    });
            });
        card.Item().PageBreak();
    }
}