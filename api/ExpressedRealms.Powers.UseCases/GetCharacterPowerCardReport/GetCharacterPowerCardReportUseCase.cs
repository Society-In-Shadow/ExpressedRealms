using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Wealth;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.CashCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.PrimaVoidCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportUseCase(
    IPowerPathRepository repository,
    ICharacterRepository characterRepository,
    ICharacterPowerRepository mappingRepository,
    IWealthRepository wealthRepository,
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

        var cards = powerCards
            .Concat(factionPowerCards)
            .Select(x => new DataCard() { CardType = CardType.PowerCard, CardData = x })
            .ToList();

        if (model.IncludeWealthCard)
        {
            await CalculateWealthCardData(model, expression.Value.Name, cards);
            // Cludgy work around - shouldn't display this if the wealth card isn't being shown
            cards.Add(await GetPrimaVoidCards(model));
        }

        var reportStream = PowerCardReport.GenerateSixUpPdf(
            cards,
            model.IsFiveByThree,
            model.IncludeWealthCard
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
        var data = await repository.GetPowerPathAndPowers(
            selectedPowerInformation.Select(x => x.PowerId).ToList()
        );

        var powerCards = data
            .Value.SelectMany(x =>
                x.Powers.Select(y => new PowerCardData()
                    {
                        AreaOfEffect = y.AreaOfEffect.Name,
                        Name = y.Name,
                        Category = y.Category?.Select(z => z.Name).ToList(),
                        Description = y.Description,
                        PathName = x.Name,
                        GameMechanicEffect = y.GameMechanicEffect,
                        ExpressionName = expression.Value.Expression,
                        PowerActivationType = y.PowerActivationType.Name,
                        PowerDuration = y.PowerDuration.Name,
                        PowerLevel = y.PowerLevel.Name,
                        Cost = y.Cost,
                        Id = y.Id,
                        IsPowerUse = y.IsPowerUse,
                        Limitation = y.Limitation,
                        Other = y.Other,
                        UserNotes =
                            selectedPowerInformation
                                .FirstOrDefault(x => x.PowerId == y.Id)
                                ?.UserNotes ?? null,
                        Prerequisites = y.Prerequisites is not null
                            ? new PrerequisiteData()
                            {
                                Count = y.Prerequisites.RequiredAmount,
                                PrerequisiteNames = y.Prerequisites.Powers,
                            }
                            : null,
                    })
                    .ToList()
            )
            .ToList();
        return powerCards;
    }

    private async Task<DataCard> GetPrimaVoidCards(
        GetCharacterPowerCardReportModel model
    )
    {
        var character = await characterRepository.FindCharacterAsync(model.CharacterId);

        return new DataCard()
        {
            CardType = CardType.PrimaVoidCard,
            CardData = new PrimaVoidCardData()
            {
                Motes = character!.Motes
            }
        };
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

    private async Task CalculateWealthCardData(
        GetCharacterPowerCardReportModel model,
        string characterName,
        List<DataCard> cards
    )
    {
        // Grab Blessings
        var wealthInfo = await wealthRepository.GetWealthInfoAsync(model.CharacterId);

        var wealthLevels = wealthInfo
            .WealthTable.Select(x => new WealthTableLine()
            {
                CashToLevelUp = x.CashToLevelUp,
                Income = x.SessionIncome,
                Level = x.Level,
                LiquidationAmount = x.LiquidationValue,
            })
            .Where(x =>
                x.Level >= wealthInfo.WealthLevel - 2 && x.Level <= wealthInfo.WealthLevel + 2
            )
            .ToList();

        if (wealthInfo.WealthLevel <= 1)
        {
            for (int i = 0; i <= 5 - wealthLevels.Count; i++)
            {
                wealthLevels.Add(
                    new WealthTableLine()
                    {
                        CashToLevelUp = -1,
                        Income = -1,
                        Level = -1,
                        LiquidationAmount = -1,
                    }
                );
            }
            wealthLevels = wealthLevels.OrderBy(x => x.Level).ToList();
        }

        cards.Add(
            new DataCard()
            {
                CardType = CardType.WealthCard,
                CardData = new WealthCardData()
                {
                    InitialBasicItemIncome = wealthInfo.InitialBasicItemIncome,
                    WealthLevel = wealthInfo.WealthLevel,
                    AppliedBlessings = wealthInfo.AppliedBlessings,
                    CharacterName = characterName,
                    WealthTableLines = wealthLevels,
                },
            }
        );

        cards.Add(
            new DataCard()
            {
                CardType = CardType.CashCard,
                CardData = new CashCardData()
                {
                    ConIncome = wealthInfo
                        .WealthTable.First(x => x.Level == wealthInfo.WealthLevel)
                        .SessionIncome,
                },
            }
        );
    }
}
