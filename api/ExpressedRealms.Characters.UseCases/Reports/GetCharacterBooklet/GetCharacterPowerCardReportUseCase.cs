using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Reports.CRB;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Skills;
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
    ICharacterSkillRepository skillRepository,
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
                Traits = await GetTraits(model),
                SkillInfo = await GetSkillInfo(model),
                Powers = await GetPowerInfo(model),
            });

        reportStream.Position = 0;
        return reportStream;
    }

    private async Task<List<PowerInfo>> GetPowerInfo(GetCharacterSheetReportModel model)
    {
        var powerMappings = await mappingRepository.GetCharacterPowerInfoForCRB(model.CharacterId);

        return powerMappings.Select(x => new PowerInfo()
        {
            Name = x.Name,
            Level = x.Level,
            XPCost = x.Exp.ToString()
        }).ToList();
        
    }

    private async Task<SkillInfo> GetSkillInfo(GetCharacterSheetReportModel model)
    {
        var skills = await skillRepository.GetCharacterSkills(model.CharacterId);

        var skillInfo = new SkillInfo()
        {
            HandToHandOffense = skills.First(x => x.SkillTypeId == (byte)SkillTypes.HandToHandOffense).LevelNumber,
            MeleeOffense = skills.First(x => x.SkillTypeId == (byte)SkillTypes.MeleeOffense).LevelNumber,
            Marksmanship = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Marksmanship).LevelNumber,
            ThrownWeapons = skills.First(x => x.SkillTypeId == (byte)SkillTypes.ThrownWeapons).LevelNumber,
            Spellcasting = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Spellcasting).LevelNumber,
            Projection = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Projection).LevelNumber,
            HandToHandDefense = skills.First(x => x.SkillTypeId == (byte)SkillTypes.HandToHandDefense).LevelNumber,
            MeleeDefense = skills.First(x => x.SkillTypeId == (byte)SkillTypes.MeleeDefense).LevelNumber,
            Acrobatics = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Acrobatics).LevelNumber,
            Spellwarding = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Spellwarding).LevelNumber,
            Deflection = skills.First(x => x.SkillTypeId == (byte)SkillTypes.Deflection).LevelNumber
        };
        return skillInfo;
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
