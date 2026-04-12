using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPlayer;

internal sealed class GetPlayerUseCase(
    IUsersRepository playerRepository,
    GetPlayerModelValidator validator,
    CancellationToken cancellationToken
) : IGetPlayerUseCase
{
    public async Task<Result<PlayerBasicInfoReturnModel>> ExecuteAsync(GetPlayerModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var player = await playerRepository.GetPlayerBasicInfoAsync(model.Id);

        return Result.Ok(new PlayerBasicInfoReturnModel()
        {
            PlayerNumber = player.PlayerNumber ?? 0
        });
    }
}
