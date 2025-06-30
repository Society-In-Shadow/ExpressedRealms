using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.DeletePrerequisite;

public class DeletePrerequisiteUseCase(
    DeletePrerequisiteModelValidator validator,
    IPowerPrerequisitesRepository repository,
    CancellationToken cancellationToken
) : IDeletePrerequisiteUseCase
{
    public async Task<Result> ExecuteAsync(DeletePrerequisiteModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await repository.RemovePrerequisitePowers(model.Id);

        await repository.DeletePrerequisite(model.Id);

        return Result.Ok();
    }
}
