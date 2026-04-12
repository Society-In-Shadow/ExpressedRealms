using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.UpdatePlayer;

internal sealed class UpdatePlayerUseCase(
    IPlayerRepository playerRepository,
    UpdatePlayerModelValidator validator,
    CancellationToken cancellationToken
) : IUpdatePlayerUseCase
{
    public async Task<Result> ExecuteAsync(UpdatePlayerModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await playerRepository.FindPlayerAsync(model.Id);

        character!.PlayerNumber = model.PlayerNumber == 0 ? null : model.PlayerNumber;

        await playerRepository.EditAsync(character);

        return Result.Ok();
    }
}
