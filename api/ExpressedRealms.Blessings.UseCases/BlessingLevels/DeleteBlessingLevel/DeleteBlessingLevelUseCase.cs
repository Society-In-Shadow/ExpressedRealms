using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.DeleteBlessingLevel;

internal sealed class DeleteBlessingLevelUseCase(
    IBlessingRepository blessingRepository,
    DeleteBlessingLevelModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteBlessingLevelUseCase
{
    public async Task<Result> ExecuteAsync(DeleteBlessingLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var level = await blessingRepository.GetBlessingLevelForEditing(model.BlessingId, model.LevelId);

        level.SoftDelete();

        await blessingRepository.EditBlessingLevelAsync(level);
        return Result.Ok();
    }
}
