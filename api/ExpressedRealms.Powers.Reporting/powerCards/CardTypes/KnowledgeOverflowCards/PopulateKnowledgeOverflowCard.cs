using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.KnowledgeOverflowCards;

internal static class PopulateKnowledgeOverflowCard
{
    public static void FillCard(ColumnDescriptor card, KnowledgeOverflowCardData data)
    {
        card.Item()
            .PaddingTop(15)
            .PaddingLeft(15)
            .PaddingRight(15)
            .PaddingBottom(7)
            .RotateLeft()
            .Decoration(decorator =>
            {
                decorator
                    .Before()
                    .Row(row =>
                {
                    row.RelativeItem()
                        .Column(header =>
                        {
                            header.Item()
                                .Text("Knowledge Overflow Card")
                                .Bold()
                                .FontSize(11)
                                .ExtraBold();
                            
                            header
                                .Item()
                                .PaddingRight(5)
                                .Row(costRow =>
                                {
                                    costRow
                                        .RelativeItem()
                                        .PaddingBottom(5)
                                        .PaddingTop(5)
                                        .AlignLeft()
                                        .Text("Name - Specialization")
                                        .Bold();

                                    costRow
                                        .ConstantItem(0.5f, Unit.Inch)
                                        .PaddingBottom(5)
                                        .PaddingTop(5)
                                        .AlignBottom()
                                        .AlignRight()
                                        .Text("Level")
                                        .Bold();
                                });
                            
                        });

                    decorator
                        .Content()
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .PaddingRight(5)
                                .Column(rightSide =>
                                {
                                    foreach (var knowledge in data.Knowledges)
                                    {
                                        rightSide
                                            .Item()
                                            .BorderBottom(0.5f)
                                            .Row(costRow =>
                                            {
                                                costRow
                                                    .RelativeItem()
                                                    .PaddingTop(2)
                                                    .PaddingBottom(2)
                                                    .AlignMiddle()
                                                    .Text(knowledge.Name)
                                                    .FontSize(9.6f);

                                                costRow
                                                    .ConstantItem(0.25f, Unit.Inch)
                                                    .PaddingTop(2)
                                                    .PaddingBottom(2)
                                                    .AlignMiddle()
                                                    .AlignCenter()
                                                    .Text(knowledge.Level)
                                                    .FontSize(9.6f);
                                            });
                                    }
                                });
                        });
                });
            });
        
        card.Item().PageBreak();
    }
}
