using ExpressedRealms.Characters.Repository;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;

internal sealed class GetPrimaryCharactersUseCase(ICharacterRepository characterRepository)
    : IGetPrimaryCharactersUseCase
{
    public async Task<Result<List<PrimaryCharacterReturnInfo>>> ExecuteAsync()
    {
        var primaryCharacters = await characterRepository.GetPrimaryCharactersAsync();

        return Result.Ok(
            primaryCharacters
                .Select(x => new PrimaryCharacterReturnInfo()
                {
                    Background = x.Background,
                    Expression = x.Expression,
                    Id = x.Id,
                    Name = x.Name,
                    PlayerName = x.PlayerName,
                    PlayerNumber = x.PlayerNumber,
                })
                .ToList()
        );
    }
}
