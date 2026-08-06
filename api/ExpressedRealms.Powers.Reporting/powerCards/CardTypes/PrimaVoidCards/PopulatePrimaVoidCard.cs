using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.PrimaVoidCards;

internal static class PopulatePrimaVoidCard
{
    public static void FillCard(ColumnDescriptor card, PrimaVoidCardData data)
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
                                    .Text($"Pryma / Void Card")
                                    .Bold()
                                    .FontSize(11)
                                    .ExtraBold();

                                leftSide
                                    .Item()
                                    .Text("Keep Hidden - This is sensitive information")
                                    .Italic()
                                    .FontSize(6)
                                    .FontColor(CustomColors.SecondaryTextColor);

                                leftSide
                                    .Item()
                                    .Text(
                                        " - Pryma is the life energy contained within the Spheres"
                                    );

                                leftSide
                                    .Item()
                                    .Text(
                                        " - Void is the dark energy composing the space between the Spheres"
                                    );

                                leftSide
                                    .Item()
                                    .Text(
                                        " - Pryma / Void is a sliding scale that goes from Pryma (7) to Void (-7), once you get to either extreme, talk to a GO"
                                    );

                                leftSide
                                    .Item()
                                    .Text(
                                        " - For the right, if void is taken, use a negative number, positive if Pryma"
                                    );

                                leftSide
                                    .Item()
                                    .Text(
                                        " - Player is responsible for writing down the modifier and source, then asking for the GO's initials when they are free"
                                    );

                                leftSide
                                    .Item()
                                    .Text(
                                        " - During Checkout, final number will be stored on the character sheet permanently"
                                    );
                            });

                        row.RelativeItem()
                            .PaddingRight(5)
                            .Column(rightSide =>
                            {
                                rightSide
                                    .Item()
                                    .Row(costRow =>
                                    {
                                        costRow
                                            .ConstantItem(0.5f, Unit.Inch)
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .AlignCenter()
                                            .Text("Mod.")
                                            .Bold();

                                        costRow
                                            .RelativeItem()
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .AlignBottom()
                                            .Text("Source")
                                            .Bold();

                                        costRow
                                            .ConstantItem(0.5f, Unit.Inch)
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .AlignCenter()
                                            .Text("Initials")
                                            .Bold();
                                    });

                                rightSide
                                    .Item()
                                    .Row(initialIncomeRow =>
                                    {
                                        initialIncomeRow
                                            .ConstantItem(0.5f, Unit.Inch)
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .AlignBottom()
                                            .BorderBottom(1)
                                            .AlignCenter()
                                            .Text(data.Motes);

                                        initialIncomeRow
                                            .RelativeItem()
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .AlignBottom()
                                            .BorderBottom(1)
                                            .Text("Starting Pryma / Void");

                                        initialIncomeRow
                                            .ConstantItem(0.5f, Unit.Inch)
                                            .PaddingLeft(5)
                                            .PaddingRight(5)
                                            .PaddingTop(5)
                                            .BorderBottom(1)
                                            .AlignCenter()
                                            .Text("OoA");
                                    });

                                foreach (var rowCount in Enumerable.Range(1, 7))
                                {
                                    rightSide
                                        .Item()
                                        .PaddingTop(5)
                                        .Row(costRow =>
                                        {
                                            costRow
                                                .ConstantItem(0.5f, Unit.Inch)
                                                .PaddingLeft(5)
                                                .PaddingRight(5)
                                                .PaddingTop(5)
                                                .BorderBottom(1)
                                                .AlignBottom()
                                                .Text("");

                                            costRow
                                                .RelativeItem()
                                                .PaddingLeft(5)
                                                .PaddingRight(5)
                                                .PaddingTop(5)
                                                .BorderBottom(1)
                                                .AlignBottom()
                                                .Text("");

                                            costRow
                                                .ConstantItem(0.5f, Unit.Inch)
                                                .PaddingLeft(5)
                                                .PaddingRight(5)
                                                .PaddingTop(5)
                                                .BorderBottom(1)
                                                .AlignBottom()
                                                .Text("");
                                        });
                                }
                            });
                    });
            });
        card.Item().PageBreak();
    }
}
