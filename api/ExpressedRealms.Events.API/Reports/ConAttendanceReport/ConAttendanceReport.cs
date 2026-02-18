using ExpressedRealms.Shared.Reports;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace ExpressedRealms.Events.API.Reports.ConAttendanceReport;

public static class ConAttendanceReport
{
    public static Document GenerateReport(ExpressionCmsReportData data)
    {
        Settings.License = LicenseType.Community;

        data.Attendees = data.Attendees.OrderBy(x => x).ToList();

        return GetSingleTilePerPage(data);
    }

    private static Document GetSingleTilePerPage(ExpressionCmsReportData data)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.DefaultTextStyle(x => x.FontSize(7.75f));
                page.Margin(0.75f, Unit.Inch);
                page.MarginTop(0.25f, Unit.Inch);
                page.MarginBottom(0.5f, Unit.Inch);

                var header = $"{data.ConName} Attendance Report for Society in Shadows";

                page.Header().AlignCenter().PaddingBottom(10).Text(header).FontSize(10).ExtraBold();

                page.Content()
                    .Column(col =>
                    {
                        foreach (var attendee in data.Attendees)
                        {
                            col.Item().Text(attendee).FontSize(10f);
                        }
                    });
                
                CommonElements.AddFooter(page);
            });
        });
    }
}
