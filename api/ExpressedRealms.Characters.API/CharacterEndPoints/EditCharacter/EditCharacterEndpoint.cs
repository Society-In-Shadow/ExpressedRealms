using ExpressedRealms.Characters.UseCases.Characters.EditKnowledge;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacter;

internal static class EditCharacterEndpoint
{
    internal static async Task<Results<NotFound, NoContent, ValidationProblem>> 
        Execute(int id, [FromBody]EditCharacterRequest dto, [FromServices]IEditCharacterUseCase repository)
    {
        var status = await repository.ExecuteAsync(
            new EditCharacterModel()
            {
                Name = dto.Name,
                Background = dto.Background,
                FactionId = dto.FactionId,
                Id = id,
                IsPrimaryCharacter = dto.IsPrimaryCharacter,
                PrimaryProgressionId = dto.PrimaryProgressionId,
                SecondaryProgressionId = dto.SecondaryProgressionId,
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
