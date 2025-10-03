using ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetCRB;

internal static class GetExpressionBookletEndpoint
{
    internal static async Task<Results<NotFound, FileStreamHttpResult, StatusCodeHttpResult, ValidationProblem>> 
        Execute(int characterId, IGetCharacterBookletUseCase repository)
    {
        var status = await repository.ExecuteAsync(
            new GetCharacterBookletModel() { CharacterId = characterId }
        );

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.File(
            status.Value,
            contentType: "application/pdf",
            fileDownloadName: "characterBookletReport.pdf",
            enableRangeProcessing: true
        );
    }
}
