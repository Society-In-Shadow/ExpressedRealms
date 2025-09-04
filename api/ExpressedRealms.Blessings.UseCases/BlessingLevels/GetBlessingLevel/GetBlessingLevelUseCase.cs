using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;

internal sealed class GetBlessingLevelUseCase(
    IBlessingRepository blessingRepository,
    GetBlessingLevelModelValidator validator,
    CancellationToken cancellationToken
) : IGetBlessingLevelUseCase
{
    public async Task<Result<GetBlessingLevelReturnModel>> ExecuteAsync(GetBlessingLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var blessing = await blessingRepository.GetBlessingLevelForEditing(
            model.BlessingId,
            model.LevelId
        );

        return Result.Ok(
            new GetBlessingLevelReturnModel()
            {
                Level = blessing.Level,
                BlessingId = blessing.BlessingId,
                XpCost = blessing.XpCost,
                XpGain = blessing.XpGain,
                Description = blessing.Description,
                Id = blessing.Id,
            }
        );
    }
}
