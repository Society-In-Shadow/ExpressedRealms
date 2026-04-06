using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Reporting.powerCards.CardTypes;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportUseCase(
    IPowerPathRepository repository,
    ICharacterRepository characterRepository,
    ICharacterPowerRepository mappingRepository,
    ICharacterBlessingRepository blessingRepository,
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

        var powerCards = data.Value.SelectMany(x =>
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

        var cards = powerCards.Select(x => new DataCard()
        {
            CardType = CardTypeEnum.PowerCard,
            CardData = x
        }).ToList();

        if (model.IncludeWealthCard)
        {
            await CalculateWealthCardData(model, cards);
        }

        var reportStream = PowerCardReport.GenerateSixUpPdf(
            cards,
            model.IsFiveByThree,
            model.IncludeWealthCard
        );

        reportStream.Position = 0;
        return reportStream;
    }

    private async Task CalculateWealthCardData(GetCharacterPowerCardReportModel model, List<DataCard> cards)
    {
        // Grab Blessings
        var blessings = await blessingRepository.GetBlessingsForCharacter(model.CharacterId);
            
        var destitute = blessings.FirstOrDefault(x =>
            x.Name == "Destitute"
        );
        var wealthy = blessings.FirstOrDefault(x =>
            x.Name == "Wealthy"
        );
            
        var wealthLevel = 1;
        double incomeModifier = 1;
            
        if (destitute is not null)
        {
            switch (destitute.LevelName)
            {
                case "2pt":
                    incomeModifier = 0.75;
                    break;
                case "4pt":
                    incomeModifier = 0.5;
                    break;
                case "6pt":
                    incomeModifier = 0.25;
                    break;
                case "8pt":
                    wealthLevel = 0;
                    incomeModifier = 0;
                    break;
            }
        }

        if (wealthy is not null)
        {
            switch (wealthy.LevelName)
            {
                case "2pt":
                    wealthLevel = 3;
                    break;
                case "4pt":
                    wealthLevel = 4;
                    incomeModifier += 0.05;
                    break;
                case "6pt":
                    wealthLevel = 5;
                    incomeModifier += 0.1;
                    break;
                case "8pt":
                    wealthLevel = 6;
                    incomeModifier += 0.25;
                    break;
            }
        }

        var sessionIncome = wealthLevel switch
        {
            0 => 0,
            1 => 50,
            2 => 100,
            _ => 2 ^ (wealthLevel - 1) * 100
        };

        var wealthIncome = sessionIncome * incomeModifier;
            
        cards.Add(new DataCard()
        {
            CardType = CardTypeEnum.WealthCard,
            CardData = new WealthCardData()
            {
                IsDestitute = destitute is not null,
                WealthIncome = wealthIncome,
                BankedCash = wealthIncome * 30,
                Liquadation = wealthIncome * 15,
                InitialBasicItemIncome = wealthIncome * 3,
                WealthLevel = wealthLevel
            }
        });
    }
}
