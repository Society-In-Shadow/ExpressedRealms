using ExpressedRealms.Admin.UseCases.UpdateCharacterXp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.AdminCharacterList.UpdateCharacterXp;

public static class UpdateCharacterXpEndpoint
{
    public static async Task<Ok> Execute (
        int characterId,
        [FromBody]UpdateCharacterXpRequest request,
        [FromServices] IUpdateCharacterXpUseCase useCase
    )
    {
        await useCase.ExecuteAsync(new UpdateCharacterXpModel()
        {
            Id = characterId,
            Xp = request.Xp,
        });

        return TypedResults.Ok();
    }
}
