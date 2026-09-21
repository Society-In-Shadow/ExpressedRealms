using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;

internal sealed class GetPrimaryCharactersUseCase(
    ICharacterRepository characterRepository,
    IEventCheckinRepository checkinRepository
) : IGetPrimaryCharactersUseCase
{
    public async Task<Result<List<PrimaryCharacterReturnInfo>>> ExecuteAsync()
    {
        var primaryCharacters = await characterRepository.GetPrimaryCharactersAsync();

        return Result.Ok(
            primaryCharacters
                .Select(x => new PrimaryCharacterReturnInfo()
                {
                    Expression = x.Expression,
                    Id = x.Id,
                    Name = x.Name,
                    PlayerName = x.PlayerName,
                    PlayerNumber = x.PlayerNumber,
                    PlayerStageId = x.PlayerStageId,
                    HasPromotionRequest = x.HasPromotionRequest,
                })
                .ToList()
        );
    }
}
