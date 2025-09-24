using ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.AdminCharacterList.GetCharacterList;

public static class GetCharacterListEndpoint
{
    public static async Task<Ok<List<PrimaryCharacter>>> Execute (
        IGetPrimaryCharactersUseCase useCase
    )
    {
        var primaryCharacters = await useCase.ExecuteAsync();

        return TypedResults.Ok(primaryCharacters.Value.Select(x => new PrimaryCharacter()
        {
            PlayerName = x.PlayerName,
            AssignedXp = x.AssignedXp,
            Background = x.Background,
            Expression = x.Expression,
            Id = x.Id,
            Name = x.Name
        }).ToList());
    }
}
