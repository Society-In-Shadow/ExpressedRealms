using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers.ReturnModels;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers;

internal sealed class GetCharacterPowersUseCase(
    ICharacterPowerRepository mappingRepository,
    IPowerPathRepository powerPathRepository,
    GetCharacterPowersModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterPowersUseCase
{
    public async Task<Result<List<PowerPathReturnModel>>> ExecuteAsync(
        GetCharacterPowersModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var powerMappingInfo = await mappingRepository.GetCharacterPowerMappingInfo(
            model.CharacterId
        );

        var powerIds = powerMappingInfo.Select(x => x.PowerId).ToList();
        var powers = await powerPathRepository.GetPowerPathAndPowers(powerIds);
        var requiredPowers = await mappingRepository.GetPowersThatArePrerequisites(model.CharacterId);

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
                            UserNotes =
                                powerMappingInfo.FirstOrDefault(z => z.PowerId == y.Id)?.UserNotes
                                ?? null,
                            Prerequisites = y.Prerequisites is not null
                                ? new PrerequisiteDetailsReturnModel()
                                {
                                    RequiredAmount = y.Prerequisites.RequiredAmount,
                                    Powers = y.Prerequisites.Powers,
                                }
                                : null,
                            RequiredPower = requiredPowers.Contains(y.Id)
                        })
                        .ToList(),
                })
                .ToList()
        );
    }
}
