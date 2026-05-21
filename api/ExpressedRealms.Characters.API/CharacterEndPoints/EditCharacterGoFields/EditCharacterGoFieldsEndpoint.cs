using ExpressedRealms.Characters.UseCases.Characters.EditCharacterGoFields;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacterGoFields;

internal static class EditCharacterGoFieldsEndpoint
{
    internal static async Task<Results<NotFound, NoContent, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] EditCharacterGoFieldsRequest dto,
        [FromServices] IEditCharacterGoFieldsUseCase repository
    )
    {
        var status = await repository.ExecuteAsync(
            new EditCharacterGoFieldsModel()
            {
                Id = characterId,
                WealthLevel = dto.WealthLevel,
                VoidFragments = dto.VoidFragments,
                Motes = dto.Motes,
                PrimaFragments = dto.PrimaFragments,
            }
        );

        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasValidationError(out var validationProblem))
            return validationProblem;
        status.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
