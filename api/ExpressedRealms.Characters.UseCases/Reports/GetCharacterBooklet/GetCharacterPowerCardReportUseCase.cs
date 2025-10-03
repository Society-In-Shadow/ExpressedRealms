using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Reports.CRB;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public class GetCharacterSheetReportUseCase(
    IPowerPathRepository repository,
    ICharacterRepository characterRepository,
    IXpRepository xpRepository,
    ICharacterPowerRepository mappingRepository,
    ICharacterBlessingRepository blessingRepository,
    GetCharacterSheetReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterSheetReportUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetCharacterSheetReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var reportStream = CharacterReferenceBookletReport.GenerateReport(new ReportData()
            {
                BasicInfo = await GetBasicInfo(model),
                Traits = await GetTraits(model)
            });

        reportStream.Position = 0;
        return reportStream;
    }

    private async Task<Traits> GetTraits(GetCharacterSheetReportModel model)
    {
        var blessings = await blessingRepository.GetBlessingsForCharacter(model.CharacterId);

        var trait = new Traits()
        {
            Advantages = blessings
                .Where(x => x.Type.Equals("Advantage", StringComparison.CurrentCultureIgnoreCase))
                .Select(x => new TraitInfo()
                {
                    Name = x.Name,
                    Cost = x.LevelName
                }).ToList(),
            Disadvantages = blessings
                .Where(x => x.Type.Equals("Disadvantage", StringComparison.CurrentCultureIgnoreCase))
                .Select(x => new TraitInfo()
                {
                    Name = x.Name,
                    Cost = x.LevelName
                }).ToList()
        };
        return trait;
    }

    private async Task<BasicInfo> GetBasicInfo(GetCharacterSheetReportModel model)
    {
        var character = await characterRepository.GetCharacterInfoForCRB(model.CharacterId);
        var characterLevel = await xpRepository.GetCharacterXpLevel(model.CharacterId);

        var basicInfo = new BasicInfo()
        {
            CharacterName = character.CharacterName,
            Expression = character.Expression,
            ProgressionPath = string.IsNullOrWhiteSpace(character.SecondaryProgressionName) 
                ? character.PrimaryProgressionName ?? "None" : 
                $"{character.PrimaryProgressionName} Secondary {character.SecondaryProgressionName}",
            PlayerNumber = character.PlayerNumber.ToString("D3"),
            PlayerName = character.PlayerName,
            CharacterLevel = characterLevel.ToString()
        };
        return basicInfo;
    }
}
