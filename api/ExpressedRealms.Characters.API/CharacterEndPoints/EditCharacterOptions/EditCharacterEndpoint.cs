using ExpressedRealms.Characters.UseCases.Characters.GetEditOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacterOptions;

internal static class EditCharacterOptionsEndpoint
{
    internal static async Task<
        Results<Ok<EditCharacterOptionResponse>, NotFound, NoContent, ValidationProblem>
    > Execute(int id, IGetEditOptionsUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(new GetEditOptionsModel() { Id = id });

        return TypedResults.Ok(
            new EditCharacterOptionResponse()
            {
                CanModifyPrimaryCharacter = result.Value.CanModifyPrimaryCharacter,
            }
        );
    }
}
