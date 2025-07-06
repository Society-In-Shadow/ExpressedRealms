using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;

[UsedImplicitly]
internal class CreatePrerequisiteUseCase(
    IPowerPrerequisitesRepository repository,
    CreatePrerequisiteModelValidator validator,
    CancellationToken cancellationToken
) : ICreatePrerequisiteUseCase
{
    public async Task<Result> ExecuteAsync(CreatePrerequisiteModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var newId = await repository.AddPrerequisite(
            new PowerPrerequisite()
            {
                PowerId = model.PowerId,
                RequiredAmount = model.RequiredAmount,
            }
        );

        await repository.AddPrerequisitePowers(
            model
                .PrerequisitePowerIds.Select(x => new PowerPrerequisitePower()
                {
                    PrerequisiteId = newId,
                    PowerId = x,
                })
                .ToList()
        );

        return Result.Ok();
    }
}
