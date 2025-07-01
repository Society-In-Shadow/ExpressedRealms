using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;

[UsedImplicitly]
internal class EditPrerequisiteUseCase(
    IPowerPrerequisitesRepository repository,
    EditPrerequisiteModelValidator validator,
    CancellationToken cancellationToken
) : IEditPrerequisiteUseCase
{
    public async Task<Result> ExecuteAsync(EditPrerequisiteModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var prerequisite = await repository.GetPrerequisiteForEditingAsync(model.Id);

        prerequisite.RequiredAmount = model.RequiredAmount;

        await repository.UpdatePrerequisite(prerequisite);

        await repository.RemovePrerequisitePowers(model.Id);

        await repository.AddPrerequisitePowers(
            model
                .PrerequisitePowerIds.Select(x => new PowerPrerequisitePower()
                {
                    PrerequisiteId = prerequisite.Id,
                    PowerId = x,
                })
                .ToList()
        );

        return Result.Ok();
    }
}
