using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;

internal sealed class GetPrerequisiteUseCase(
    IPowerPrerequisitesRepository repository,
    GetPrerequisiteModelValidator validator,
    CancellationToken cancellationToken
) : IGetPrerequisiteUseCase
{
    public async Task<Result<GetPrerequisiteData?>> ExecuteAsync(GetPrerequisiteModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var prerequisite = await repository.GetPrerequisiteAndPowersForEditingAsync(model.PowerId);

        if (prerequisite is null)
        {
            return Result.Ok<GetPrerequisiteData?>(null);
        }

        return Result.Ok<GetPrerequisiteData?>(
            new GetPrerequisiteData()
            {
                Id = prerequisite.Id,
                RequiredAmount = prerequisite.RequiredAmount,
                PowerIds = prerequisite.PrerequisitePowers.Select(x => x.PowerId).ToList(),
            }
        );
    }
}
