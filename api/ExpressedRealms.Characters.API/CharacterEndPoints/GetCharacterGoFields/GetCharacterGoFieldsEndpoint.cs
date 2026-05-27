using ExpressedRealms.Characters.UseCases.Characters.GetCharacterGoFields;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetCharacterGoFields;

internal static class GetCharacterGoFieldsEndpoint
{
    internal static async Task<Results<NotFound, Ok<GetCharacterGoFieldsResponse>, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromServices] IGetCharacterGoFieldsUseCase repository
    )
    {
        var response = await repository.ExecuteAsync(
            new GetCharacterGoFieldsModel()
            {
                Id = characterId,
            }
        );

        if (response.HasNotFound(out var notFound))
            return notFound;
        if (response.HasValidationError(out var validationProblem))
            return validationProblem;
        response.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new GetCharacterGoFieldsResponse()
        {
            WealthLevel = response.Value.WealthLevel,
            Motes = response.Value.Motes,
            PrimaFragments = response.Value.PrimaFragments,
            VoidFragments = response.Value.VoidFragments,
        });
    }
}
