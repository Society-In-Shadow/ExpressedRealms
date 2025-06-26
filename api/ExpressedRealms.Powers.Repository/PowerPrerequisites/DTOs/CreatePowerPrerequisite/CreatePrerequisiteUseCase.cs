using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.CreatePowerPrerequisite;

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
        
        await repository.AddPrerequisite(new PowerPrerequisite()
        {
            PowerId = model.Id,
            RequiredAmount = model.RequiredAmount,
        });

        await repository.AddPrerequisitePowers(model.PowerIds.Select(x => new PowerPrerequisitePower()
        {
            PrerequisiteId = model.Id,
            PowerId = x,
        }).ToList());
        
        return Result.Ok();
    }
}