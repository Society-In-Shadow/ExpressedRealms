using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Wealth;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes.CashCards;
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

        var cards = powerCards
            .Select(x => new DataCard() { CardType = CardType.PowerCard, CardData = x })
            .ToList();

        if (model.IncludeWealthCard)
        {
            await CalculateWealthCardData(model, expression.Value.Name, cards);
        }

        var reportStream = PowerCardReport.GenerateSixUpPdf(
            cards,
            model.IsFiveByThree,
            model.IncludeWealthCard
        );

        reportStream.Position = 0;
        return reportStream;
    }

    private async Task CalculateWealthCardData(
        GetCharacterPowerCardReportModel model,
        string characterName,
        List<DataCard> cards
    )
    {
        // Grab Blessings
        var wealthInfo = await wealthRepository.GetWealthInfoAsync(model.CharacterId);

        var wealthLevels = wealthInfo.WealthTable.Select(x => new WealthTableLine()
        {
            CashToLevelUp = x.CashToLevelUp,
            Income = x.SessionIncome,
            Level = x.Level,
            LiquidationAmount = x.LiquidationValue
        }).Where(x => x.Level >= wealthInfo.WealthLevel - 2 && x.Level <= wealthInfo.WealthLevel + 2).ToList();

        if (wealthInfo.WealthLevel <= 1)
        {
            for(int i = 0; i <= 5 - wealthLevels.Count; i++)
            {
                wealthLevels.Add(new WealthTableLine()
                {
                    CashToLevelUp = -1,
                    Income = -1,
                    Level = -1,
                    LiquidationAmount = -1
                });
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
                    WealthTableLines = wealthLevels
                },
            }
        );
        
        cards.Add(
            new DataCard()
            {
                CardType = CardType.CashCard,
                CardData = new CashCardData()
                {
                    ConIncome = wealthInfo.WealthTable.First(x => x.Level == wealthInfo.WealthLevel).SessionIncome
                },
            }
        );
    }
}
