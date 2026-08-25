using ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.GetCharacterStorageOptins;

public static class GetCharacterStorageOptinsEndpoint
{
    public static async Task<Ok<GetCharacterStorageOptinsResponse>> ExecuteAsync(
        int id,
        [FromServices] IGetCharacterStorageOptinsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetCharacterStorageOptinsModel() { EventId = id }
        );

        return TypedResults.Ok(
            new GetCharacterStorageOptinsResponse()
            {
                CharacterStorageOptins = results
                    .Value.CharacterStorageOptins.Select(x => new CharacterStorageOptin()
                    {
                        Id = x.Id,
                        ApproverName = x.ApproverName,
                        PlayerName = x.PlayerName,
                        Amount = x.Amount,
                        Timestamp = x.Timestamp,
                    })
                    .ToList(),
            }
        );
    }
}
