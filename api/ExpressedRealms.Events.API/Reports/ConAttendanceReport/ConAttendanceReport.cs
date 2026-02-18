using ExpressedRealms.Shared.Reports;
using QuestPDF;
using QuestPDF.Fluent;
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
                var header = $"{data.ConName} Attendance Report for Society in Shadows";
                CommonElements.AddHeader(page, header);

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
