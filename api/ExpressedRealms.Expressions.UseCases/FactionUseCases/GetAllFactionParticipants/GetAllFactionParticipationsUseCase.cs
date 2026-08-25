using ExpressedRealms.Expressions.Repository.Factions;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetAllFactionParticipants;

internal sealed class GetAllFactionParticipants(IFactionRepository factionRepository)
    : IGetAllFactionParticipants
{
    public async Task<Result<GetAllFactionParticipantsReturnModel>> ExecuteAsync()
    {
        var expressions = await factionRepository.GetExpressionFactions();

        return Result.Ok(
            new GetAllFactionParticipantsReturnModel()
            {
                Expressions =
                [
                    .. expressions.Select(x => new ExpressionDto()
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
