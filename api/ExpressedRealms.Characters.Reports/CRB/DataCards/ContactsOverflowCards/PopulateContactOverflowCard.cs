using ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;
using ExpressedRealms.Shared;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB.DataCards.ContactsOverflowCards;

public class PopulateContactOverflowCard : ICardTile
{
    private readonly ContactsOverflowCardData _cardData;

    public PopulateContactOverflowCard(ContactsOverflowCardData data)
    {
        _cardData = data;
    }

    public void Populate(ColumnDescriptor col)
    {
        col.Item()
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
                                header
                                    .Item()
                                    .Text("Contacts Overflow Card")
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
                                            .Text("Name - Knowledge - Knowledge Level")
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
                                        foreach (var knowledge in _cardData.Contacts)
                                        {
                                            rightSide
                                                .Item()
                                                .Row(costRow =>
                                                {
                                                    costRow
                                                        .RelativeItem()
                                                        .PaddingTop(2)
                                                        .PaddingBottom(5)
                                                        .AlignMiddle()
                                                        .Text($"{knowledge.Name.Limit(25, ".")} - {knowledge.KnowledgeName.Limit(30, ".")} - {knowledge.KnowledgeLevel}");
                                                });
                                            
                                            rightSide
                                                .Item()
                                                .AlignCenter()
                                                .AlignMiddle()
                                                .PaddingBottom(2)
                                                .Row(costRow =>
                                                {
                                                    CreateStamp(costRow.RelativeItem(), string.Empty);
                                                    CreateStamp(costRow.RelativeItem(), knowledge.NumberOfUses >= 2 ? string.Empty : "X");
                                                    CreateStamp(costRow.RelativeItem(), knowledge.NumberOfUses >= 3 ? string.Empty : "X");
                                                });
                                        }
                                        
                                        
                                    });
                            });
                    });
            });

        col.Item().PageBreak();
    }
    
    private static void CreateStamp(IContainer initialIncomeRow, string stampText)
    {
        initialIncomeRow
            .Width(0.67f, Unit.Inch)
            .Height(0.67f, Unit.Inch)
            .Border(1)
            .BorderLinearGradient(0, [Color.FromARGB(255, 0, 0, 0)])
            .AlignCenter()
            .AlignMiddle()
            .Text(stampText)
            .FontSize(36);
    }
}
