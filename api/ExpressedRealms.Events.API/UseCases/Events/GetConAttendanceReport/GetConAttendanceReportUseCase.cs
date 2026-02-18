using ExpressedRealms.Events.API.Reports.ConAttendanceReport;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using QuestPDF.Fluent;

namespace ExpressedRealms.Events.API.UseCases.Events.GetConAttendanceReport;

internal sealed class GetConAttendanceReportUseCase(
    IEventRepository eventRepository,
    GetConAttendanceReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetEventAttendanceReportUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetConAttendanceReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var conEvent = await eventRepository.GetEventAsync(model.Id);
        var conAttendees = await eventRepository.GetRegisteredAttendeesAsync(conEvent.Id);

        var report = ConAttendanceReport.GenerateReport(
            new() { ConName = conEvent.Name, Attendees = conAttendees }
        );

        var stream = new MemoryStream();
        report.GeneratePdf(stream);

        stream.Position = 0;
        return stream;
    }
}
