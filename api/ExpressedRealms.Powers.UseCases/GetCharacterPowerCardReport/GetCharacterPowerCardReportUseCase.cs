using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportUseCase(
    IPowerPathRepository repository,
    ICharacterRepository characterRepository,
    ICharacterPowerRepository mappingRepository,
    GetCharacterPowerCardReportModelValidator validator,
    ICharacterFactionRepository factionRepository,
    CancellationToken cancellationToken
) : IGetCharacterPowerCardReportUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetCharacterPowerCardReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var expression = await characterRepository.GetCharacterInfoAsync(model.CharacterId);

        var powerCards = await GetPowerCardData(model, expression);
        var factionPowerCards = await GetFactionPowerCards(model);

        var reportStream = PowerCardReport.GenerateSixUpPdf(
            [.. powerCards, .. factionPowerCards],
            model.IsFiveByThree,
            model.CardTiles
        );

        reportStream.Position = 0;
        return reportStream;
    }

    private async Task<List<PowerCardData>> GetPowerCardData(
        GetCharacterPowerCardReportModel model,
        Result<GetEditCharacterDto> expression
    )
    {
        var selectedPowerInformation = await mappingRepository.GetCharacterPowerMappingInfo(
            model.CharacterId
        );
        var data = await repository.GetPowerPathAndPowersForCrb(
            selectedPowerInformation.Select(x => x.PowerId).ToList()
        );

        var powerCards = data.Select(y => new PowerCardData()
            {
                AreaOfEffect = y.AreaOfEffect,
                Name = y.Name,
                Category = y.Category?.ToList(),
                Description = y.Description,
                PathName = y.PathName,
                GameMechanicEffect = y.GameMechanicEffect,
                ExpressionName = expression.Value.Expression,
                PowerActivationType = y.PowerActivationType,
                PowerDuration = y.PowerDuration,
                PowerLevel = y.PowerLevel,
                Cost = y.Cost,
                Id = y.Id,
                IsPowerUse = y.IsPowerUse,
                Limitation = y.Limitation,
                Other = y.Other,
                UserNotes =
                    selectedPowerInformation.FirstOrDefault(x => x.PowerId == y.Id)?.UserNotes
                    ?? null,
                Prerequisites = y.Prerequisites is not null
                    ? new PrerequisiteData()
                    {
                        Count = y.Prerequisites.RequiredAmount,
                        PrerequisiteNames = y.Prerequisites.Powers,
                    }
                    : null,
            })
            .ToList();
        return powerCards;
    }

    private async Task<List<PowerCardData>> GetFactionPowerCards(
        GetCharacterPowerCardReportModel model
    )
    {
        var factionInfo = await factionRepository.GetPlayerFactionInfo(model.CharacterId);
        if (factionInfo == null)
            return [];

        var factionPowerData = await factionRepository.GetAppliedFactionPowerIds(model.CharacterId);
        var factionPowerIds = factionPowerData.Select(x => x.PowerId).ToList();
        var factionPowers = await repository.GetPowers(factionPowerIds);

        var lookup = factionPowers.ToDictionary(x => x.Id);

        var sortedFactionPowers = factionPowerIds
            .Where(lookup.ContainsKey)
            .Select(id => lookup[id])
            .ToList();

        var factionPowerCards = sortedFactionPowers
            .Select(y => new PowerCardData()
            {
                AreaOfEffect = y.AreaOfEffect.Name,
                Name = y.Name,
                Category = y.Category?.Select(z => z.Name).ToList(),
                Description = y.Description,
                PathName = factionPowerData.First(x => x.PowerId == y.Id).FactionRankName,
                GameMechanicEffect = y.GameMechanicEffect,
                ExpressionName = factionInfo.FactionName,
                PowerActivationType = y.PowerActivationType.Name,
                PowerDuration = y.PowerDuration.Name,
                PowerLevel = y.PowerLevel.Name,
                Cost = y.Cost,
                Id = y.Id,
                IsPowerUse = y.IsPowerUse,
                Limitation = y.Limitation,
                Other = y.Other,
            })
            .ToList();
        return factionPowerCards;
    }
}
