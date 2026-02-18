using System.Runtime.InteropServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Shared.Reports;

public static class CommonElements
{
    public static void AddFooter(PageDescriptor page)
    {
        page.Footer()
            .Row(row =>
            {
                        
                var tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Central Standard Time" // Windows ID
                    : "America/Chicago"; // IANA ID

                var central = TimeZoneInfo.FindSystemTimeZoneById(tzId);
                DateTimeOffset nowCentral = TimeZoneInfo.ConvertTime(
                    DateTimeOffset.UtcNow,
                    central
                );

                row.RelativeItem()
                    .Column(x =>
                    {
                        x.Item().Text($"©{nowCentral.Year} Expressed Realms");
                        x.Item().Text("Society in Shadows LARP. All rights reserved.");
                    });

                row.RelativeItem()
                    .AlignCenter()
                    .Column(x =>
                    {
                        x.Item().Text("");
                        x.Item()
                            .Text(text =>
                            {
                                text.Span("Page ");
                                text.CurrentPageNumber();
                                text.Span(" / ");
                                text.TotalPages();
                            });
                    });
                row.RelativeItem()
                    .AlignRight()
                    .Column(x =>
                    {
                        x.Item().Text("");
                        x.Item().Text($"Download Date: {nowCentral:M/d/yyyy h:mm tt} CST");
                    });
            });
    }


    public static void AddHeader(PageDescriptor page, string title)
    {
        page.Size(PageSizes.Letter);
        page.DefaultTextStyle(x => x.FontSize(7.75f));
        page.Margin(0.75f, Unit.Inch);
        page.MarginTop(0.25f, Unit.Inch);
        page.MarginBottom(0.5f, Unit.Inch);


        page.Header().AlignCenter().PaddingBottom(10).Text(title).FontSize(10).ExtraBold();
    }
}