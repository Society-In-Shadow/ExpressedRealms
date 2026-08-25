using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetAllFactionParticipants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFactionParticipants;

public static class GetAllFactionParticipantsEndpoint
{
    public static async Task<Ok<FactionParticipationResponse>> ExecuteAsync(
        IGetAllFactionParticipants createFactionUseCase
    )
    {
        var results = await createFactionUseCase.ExecuteAsync();

        return TypedResults.Ok(
            new FactionParticipationResponse()
            {
                Expressions =
                [
                    .. results.Value.Expressions.Select(x => new ExpressionDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Factions =
                        [
                            .. x.Factions.Select(y => new FactionDto()
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Players =
                                [
                                    .. y.Players.Select(a => new PlayerDto()
                                    {
                                        Id = a.Id,
                                        CharacterName = a.CharacterName,
                                        Level = a.Id,
                                        LevelName = a.LevelName,
                                        Player = a.Player,
                                    }),
                                ],
                            }),
                        ],
                    }),
                ],
            }
        );
    }
}
