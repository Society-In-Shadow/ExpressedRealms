using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.CashCards;

internal static class PopulateCashCard
{
    public static void FillCard(ColumnDescriptor card, CashCardData wealthData)
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
                                leftSide
                                    .Item()
                                    .Row(initialIncomeRow =>
                                    {
                                        initialIncomeRow
                                            .RelativeItem()
                                            .Text(
                                                $"Initial Cash Card - ${wealthData.ConIncome.ToString("N0")}"
                                            )
                                            .Bold()
                                            .FontSize(11)
                                            .ExtraBold();
                                    });

                                foreach (var rowCount in Enumerable.Range(1, 3))
                                {
                                    leftSide
                                        .Item()
                                        .PaddingTop(5)
                                        .Row(costRow =>
                                        {
                                            foreach (var columnCount in Enumerable.Range(1, 2))
                                            {
                                                costRow
                                                    .ConstantItem(0.67f, Unit.Inch)
                                                    .Width(0.67f, Unit.Inch)
                                                    .Height(0.67f, Unit.Inch)
                                                    .Border(1)
                                                    .BorderLinearGradient(
                                                        0,
                                                        [Color.FromARGB(255, 0, 0, 0)]
                                                    );

                                                costRow
                                                    .RelativeItem()
                                                    .PaddingLeft(5)
                                                    .PaddingRight(5)
                                                    .PaddingTop(20)
                                                    .BorderTop(1)
                                                    .BorderBottom(1)
                                                    .AlignBottom()
                                                    .Text("New Total")
                                                    .FontColor(Color.FromARGB(45, 0, 0, 0));
                                            }
                                        });
                                }
                            });
                    });
            });
        card.Item().PageBreak();
    }
}
