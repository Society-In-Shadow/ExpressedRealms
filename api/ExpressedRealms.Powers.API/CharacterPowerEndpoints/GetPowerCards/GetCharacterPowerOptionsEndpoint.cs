using ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetPowerCards;

public static class GetCharacterPowerCardReportEndpoint
{
    public static async Task<Results<FileStreamHttpResult, ValidationProblem>> 
        Execute(int characterId, bool isFiveByThree, IGetCharacterPowerCardReportUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new() { CharacterId = characterId, IsFiveByThree = isFiveByThree }
        );
        
        if (results.HasValidationError(out var validation))
            return validation;

        results.ThrowIfErrorNotHandled();

        return TypedResults.File(
            results.Value,
            contentType: "application/pdf",
            fileDownloadName: "characterPowerCardReport.pdf",
            enableRangeProcessing: true
        );
    }
}
