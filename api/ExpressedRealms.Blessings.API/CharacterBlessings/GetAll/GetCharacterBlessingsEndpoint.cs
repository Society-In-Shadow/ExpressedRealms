using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Blessings.API.CharacterBlessings.GetAll;

public static class GetCharacterBlessingsEndpoint
{
    public static async Task<Ok<CharacterBlessingsBaseResponse>> ExecuteAsync(
        int characterId,
        IGetAssignedBlessingsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetAssignedBlessingsModel() { CharacterId = characterId }
        );

        return TypedResults.Ok(
            new CharacterBlessingsBaseResponse()
            {
                Blessings = results
                    .Value.Select(x => new CharacterBlessing()
                    {
                        Description = x.Description,
                        LevelDescription = x.LevelDescription,
                        LevelName = x.LevelName,
                        Name = x.Name,
                        BlessingLevelId = x.BlessingLevelId,
                        BlessingId = x.BlessingId,
                    }).ToList()
            }
        );
    }
}
