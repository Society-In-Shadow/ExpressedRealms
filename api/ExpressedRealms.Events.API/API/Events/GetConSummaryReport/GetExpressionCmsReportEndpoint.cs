using ExpressedRealms.Events.API.UseCases.Events.GetConAttendanceReport;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.GetConSummaryReport;

internal static class GetConSummaryReportEndpoint
{
    internal static async Task<Results<NotFound, FileStreamHttpResult, StatusCodeHttpResult, ValidationProblem>
    > ExecuteAsync(int id, [FromServices] IGetEventAttendanceReportUseCase repository)
    {
        var status = await repository.ExecuteAsync(
            new () { Id = id }
        );

        if (status.HasValidationError<MemoryStream>(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.File(
            status.Value,
            contentType: "application/pdf",
            fileDownloadName: "conEventReport.pdf",
            enableRangeProcessing: true
        );
    }
}
