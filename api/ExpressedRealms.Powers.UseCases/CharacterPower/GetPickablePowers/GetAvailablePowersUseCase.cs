using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers;

internal sealed class GetAvailablePowersUseCase(
    ICharacterPowerRepository mappingRepository,
    IPowerPathRepository powerPathRepository,
    GetAvailablePowersModelValidator validator,
    CancellationToken cancellationToken
) : IGetAvailablePowersUseCase
{
    public async Task<Result<List<PowerPathReturnModel>>> ExecuteAsync(
        GetAvailablePowersModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var powerIds = await mappingRepository.GetSelectablePowersForCharacter(model.CharacterId);

        var powers = await powerPathRepository.GetPowerPathAndPowers(powerIds);

        return Result.Ok(
            powers
                .Value.Select(x => new PowerPathReturnModel()
                {
                    Name = x.Name,
                    Powers = x
                        .Powers.Select(y => new PowerReturnModel()
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Category =
                                y.Category?.Select(z => new DetailedInformationReturnModel(z))
                                    .ToList() ?? new List<DetailedInformationReturnModel>(),
                            Description = y.Description,
                            GameMechanicEffect = y.GameMechanicEffect,
                            Limitation = y.Limitation,
                            PowerDuration = new DetailedInformationReturnModel(y.PowerDuration),
                            AreaOfEffect = new DetailedInformationReturnModel(y.AreaOfEffect),
                            PowerLevel = new DetailedInformationReturnModel(y.PowerLevel),
                            PowerActivationType = new DetailedInformationReturnModel(
                                y.PowerActivationType
                            ),
                            Other = y.Other,
                            IsPowerUse = y.IsPowerUse,
                            Cost = y.Cost,
                            Prerequisites = y.Prerequisites is not null
                                ? new PrerequisiteDetailsReturnModel()
                                {
                                    RequiredAmount = y.Prerequisites.RequiredAmount,
                                    Powers = y.Prerequisites.Powers,
                                }
                                : null,
                        })
                        .ToList(),
                })
                .ToList()
        );
    }
}
