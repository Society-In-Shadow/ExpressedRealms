using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.CreateBlessingLevel;

internal sealed class CreateBlessingLevelUseCase(
    IBlessingRepository blessingRepository,
    CreateBlessingLevelModelValidator validator,
    CancellationToken cancellationToken
) : ICreateBlessingLevelUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateBlessingLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var returnedId = await blessingRepository.CreateBlessingLevelAsync(
            new BlessingLevel()
            {
                BlessingId = model.BlessingId,
                XpCost = model.XpCost,
                XpGain = model.XpGain,
                Level = model.Level,
                Description = model.Description,
            }
        );

        return Result.Ok(returnedId);
    }
}