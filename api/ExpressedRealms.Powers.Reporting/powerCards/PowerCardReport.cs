using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Powers.Reporting.powerCards;

public static class PowerCardReport
{
    public static Document GenerateReport(List<PowerCardData> powerCards)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());
                    page.DefaultTextStyle(x => x.FontSize(12));

                    float rowHeight = (PageSizes.Letter.Landscape().Height / 3);

                    // 2 rows × 3 columns of equal-sized sections
                    page.Content().Column(col =>
                    {
                        col.Spacing(0); // set to 0 for no gap between sections
                        
                        // Top row
                        col.Item().Height(rowHeight).Row(row =>
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                row.RelativeItem()
                                    .Border(1)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text($"Section {i + 1}");
                            }
                        });

                        // Bottom row
                        col.Item().Height(rowHeight).Row(row =>
                        {
                            for (int i = 2; i < 4; i++)
                            {
                                row.RelativeItem()
                                    .Border(1)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text($"Section {i + 1}");
                            }
                        });
                        
                        col.Item().Height(rowHeight).Row(row =>
                        {
                            for (int i = 4; i < 6; i++)
                            {
                                row.RelativeItem()
                                    .Border(1)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text($"Section {i + 1}");
                            }
                        });
                    });
                });
            });

        
        
        
    }
}
