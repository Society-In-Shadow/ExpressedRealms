using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.Powers.CreatePower;

internal sealed class CreatePowerUseCase(
    IPowerRepository powerRepository,
    IPowerPathRepository powerPathRepository,
    CreatePowerModelValidator validator,
    CancellationToken cancellationToken
) : ICreatePowerUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreatePowerModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var newPower = new Power
        {
            Name = model.Name,
            Description = model.Description,
            LevelId = model.PowerLevel,
            AreaOfEffectTypeId = model.AreaOfEffect,
            ActivationTimingTypeId = model.PowerActivationType,
            DurationId = model.PowerDuration,
            IsPowerUse = model.IsPowerUse,
            GameMechanicEffect = model.GameMechanicEffect,
            Limitation = model.Limitation,
            OtherFields = model.Other,
            Cost = model.Cost,
            CategoryMappings = model.Category.Select(x => new PowerCategoryMapping()
            {
                CategoryId = x,
            }).ToList()
        };

        var powerId = await powerRepository.CreatePower(newPower);

        await powerPathRepository.AddPowerToPowerPath(
            new PowerPathPowerMapping()
            {
                PowerId = powerId,
                PowerPathId = model.PowerPathId
            });
        
        return Result.Ok(powerId);
    }


}
