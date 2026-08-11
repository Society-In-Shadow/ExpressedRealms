using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.Characters.Repository.Wealth;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Shared;
using FluentResults;
using StatModifier = ExpressedRealms.Characters.Reports.CRB.Data.SupportingData.StatModifier;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public class GetCharacterSheetDataUseCase(
    ICharacterKnowledgeRepository knowledgeRepository,
    ICharacterRepository characterRepository,
    IXpRepository xpRepository,
    ICharacterPowerRepository mappingRepository,
    ICharacterBlessingRepository blessingRepository,
    ICharacterSkillRepository skillRepository,
    IProficiencyRepository proficiencyRepository,
    ICharacterStatRepository statRepository,
    IContactRepository contactRepository,
    IEventCheckinRepository eventCheckinRepository,
    IWealthRepository wealthRepository,
    ICharacterFactionRepository characterFactionRepository
) : IGetCharacterSheetDataUseCase
{
    public async Task<Result<ReportData>> ExecuteAsync(GetCharacterSheetDataReportModel model)
    {
        // This function assumes character id has already been validated

        var statInfo = await GetStatModifierInfo(model);

        return new ReportData()
        {
            BasicInfo = await GetBasicInfo(model),
            Traits = await GetTraits(model),
            SkillInfo = await GetSkillInfo(model),
            Powers = await GetPowerInfo(model),
            Knowledges = await GetKnowledgeInfo(model),
            ProficiencyInfo = await GetProficiencyInfo(model),
            StatInfo = statInfo,
            Contacts = await GetContactinfo(model),
            WealthInfo = await GetWealthInfo(model),
        };
    }

    private async Task<WealthInfoDto> GetWealthInfo(GetCharacterSheetDataReportModel model)
    {
        var wealthInfo = await wealthRepository.GetWealthInfoAsync(model.CharacterId);

        return new WealthInfoDto()
        {
            WealthIncome = wealthInfo
                .WealthTable.First(x => x.Level == wealthInfo.WealthLevel)
                .SessionIncome,
            WealthLevel = wealthInfo.WealthLevel,
        };
    }

    private async Task<StatModifierInfo> GetStatModifierInfo(GetCharacterSheetDataReportModel model)
    {
        var stats = await statRepository.GetAllStats(model.CharacterId);

        var statInfo = new StatModifierInfo()
        {
            Constitution = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Constitution).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Constitution).Bonus,
            },
            Strength = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Strength).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Strength).Bonus,
            },
            Dexterity = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Dexterity).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Dexterity).Bonus,
            },
            Intelligence = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Intelligence).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Intelligence).Bonus,
            },
            Willpower = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Willpower).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Willpower).Bonus,
            },
            Agility = new StatModifier()
            {
                Stat = stats.Value.First(x => x.StatTypeId == StatType.Agility).Level,
                Bonus = stats.Value.First(x => x.StatTypeId == StatType.Agility).Bonus,
            },
        };
        return statInfo;
    }

    private async Task<ProficiencyData> GetProficiencyInfo(GetCharacterSheetDataReportModel model)
    {
        var proficiencies = await proficiencyRepository.GetBasicProficiencies(model.CharacterId);

        var proficiencyInfo = new ProficiencyData();

        proficiencyInfo.Vitality = proficiencies.Value.First(x => x.Id == 13).Value;
        proficiencyInfo.Health = proficiencies.Value.First(x => x.Id == 14).Value;
        proficiencyInfo.Blood = proficiencies.Value.First(x => x.Id == 15).Value;
        proficiencyInfo.Reaction = proficiencies.Value.First(x => x.Id == 16).Value;
        proficiencyInfo.Psyche = proficiencies.Value.First(x => x.Id == 17).Value;
        proficiencyInfo.RWP = proficiencies.Value.First(x => x.Id == 22).Value;
        proficiencyInfo.Mortis = proficiencies.Value.First(x => x.Id == 23).Value;

        proficiencyInfo.Chi = proficiencies.Value.FirstOrDefault(x => x.Id == 18)?.Value ?? 0;
        proficiencyInfo.Essence = proficiencies.Value.FirstOrDefault(x => x.Id == 19)?.Value ?? 0;
        proficiencyInfo.Mana = proficiencies.Value.FirstOrDefault(x => x.Id == 20)?.Value ?? 0;
        proficiencyInfo.Noumenon = proficiencies.Value.FirstOrDefault(x => x.Id == 21)?.Value ?? 0;

        proficiencyInfo.Strike = proficiencies.Value.First(x => x.Id == 1).Value;
        proficiencyInfo.Dodge = proficiencies.Value.First(x => x.Id == 2).Value;
        proficiencyInfo.Thrust = proficiencies.Value.First(x => x.Id == 3).Value;
        proficiencyInfo.Parry = proficiencies.Value.First(x => x.Id == 4).Value;
        proficiencyInfo.Throw = proficiencies.Value.First(x => x.Id == 5).Value;
        proficiencyInfo.EvadeThrow = proficiencies.Value.First(x => x.Id == 6).Value;
        proficiencyInfo.Shoot = proficiencies.Value.First(x => x.Id == 7).Value;
        proficiencyInfo.EvadeShoot = proficiencies.Value.First(x => x.Id == 8).Value;
        proficiencyInfo.Cast = proficiencies.Value.First(x => x.Id == 9).Value;
        proficiencyInfo.Ward = proficiencies.Value.First(x => x.Id == 10).Value;
        proficiencyInfo.Project = proficiencies.Value.First(x => x.Id == 11).Value;
        proficiencyInfo.Deflect = proficiencies.Value.First(x => x.Id == 12).Value;

        proficiencyInfo.WalkingOffensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.WalkingOffensiveProficiency.Value)
            .Value;

        proficiencyInfo.WalkingDefensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.WalkingDefensiveProficiency.Value)
            .Value;

        proficiencyInfo.WalkingPaces = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.WalkingPaces.Value)
            .Value;

        proficiencyInfo.RunningOffensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.RunningOffensiveProficiency.Value)
            .Value;

        proficiencyInfo.RunningDefensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.RunningDefensiveProficiency.Value)
            .Value;

        proficiencyInfo.RunningPaces = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.RunningPaces.Value)
            .Value;

        proficiencyInfo.SprintingOffensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.SprintingOffensiveProficiency.Value)
            .Value;

        proficiencyInfo.SprintingDefensiveProficiencies = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.SprintingDefensiveProficiency.Value)
            .Value;

        proficiencyInfo.SprintingPaces = proficiencies
            .Value.First(x => x.Id == StatModifierEnum.SprintingPaces.Value)
            .Value;

        return proficiencyInfo;
    }

    private async Task<List<KnowledgeInfo>> GetKnowledgeInfo(GetCharacterSheetDataReportModel model)
    {
        var knowledges = await knowledgeRepository.GetKnowledgesForCharacterCRB(model.CharacterId);

        var knowledgeList = new List<KnowledgeInfo>();

        foreach (var knowledge in knowledges)
        {
            knowledgeList.Add(
                new KnowledgeInfo() { Name = knowledge.Name, Level = knowledge.Level.ToString() }
            );

            knowledgeList.AddRange(
                knowledge.Specializations.Select(spec => new KnowledgeInfo()
                {
                    Name = $" - {spec}",
                    Level = String.Empty,
                })
            );
        }
        return knowledgeList;
    }

    private async Task<List<ContactInfo>> GetContactinfo(GetCharacterSheetDataReportModel model)
    {
        var contacts = await contactRepository.GetContactsForCRB(model.CharacterId);

        return contacts
            .Select(x => new ContactInfo()
            {
                Name = x.Name,
                KnowledgeName = x.Knowledge,
                KnowledgeLevel = x.KnowledgeLevel,
                NumberOfUses = x.UsesPerWeek,
            })
            .ToList();
    }

    private async Task<List<PowerInfo>> GetPowerInfo(GetCharacterSheetDataReportModel model)
    {
        var powerMappings = await mappingRepository.GetCharacterPowerInfoForCRB(model.CharacterId);
        var factionMappings = await characterFactionRepository.GetAppliedFactionPowerInfoForCrb(
            model.CharacterId
        );

        var powers = powerMappings
            .Select(x => new PowerInfo()
            {
                Name = x.Name,
                Level = x.Level,
                XPCost = x.Exp.ToString(),
            })
            .ToList();

        var factionPowers = factionMappings
            .Select(x => new PowerInfo()
            {
                Name = x.Name,
                Level = x.Level,
                XPCost = x.XpCost,
            })
            .ToList();

        return powers.Concat(factionPowers).ToList();
    }

    private async Task<SkillInfo> GetSkillInfo(GetCharacterSheetDataReportModel model)
    {
        var skills = await skillRepository.GetCharacterSkills(model.CharacterId);

        var skillInfo = new SkillInfo()
        {
            HandToHandOffense = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.HandToHandOffense)
                .LevelNumber,
            MeleeOffense = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.MeleeOffense)
                .LevelNumber,
            Marksmanship = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Marksmanship)
                .LevelNumber,
            ThrownWeapons = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.ThrownWeapons)
                .LevelNumber,
            Spellcasting = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Spellcasting)
                .LevelNumber,
            Projection = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Projection)
                .LevelNumber,
            HandToHandDefense = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.HandToHandDefense)
                .LevelNumber,
            MeleeDefense = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.MeleeDefense)
                .LevelNumber,
            Acrobatics = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Acrobatics)
                .LevelNumber,
            Spellwarding = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Spellwarding)
                .LevelNumber,
            Deflection = skills
                .First(x => x.SkillTypeId == (byte)SkillTypes.Deflection)
                .LevelNumber,
        };
        return skillInfo;
    }

    private async Task<Traits> GetTraits(GetCharacterSheetDataReportModel model)
    {
        var blessings = await blessingRepository.GetBlessingsForCharacter(model.CharacterId);

        var trait = new Traits()
        {
            Advantages = blessings
                .Where(x => x.Type.Equals("Advantage", StringComparison.CurrentCultureIgnoreCase))
                .Select(x => new TraitInfo() { Name = x.Name, Cost = x.LevelName })
                .ToList(),
            Disadvantages = blessings
                .Where(x =>
                    x.Type.Equals("Disadvantage", StringComparison.CurrentCultureIgnoreCase)
                )
                .Select(x => new TraitInfo() { Name = x.Name, Cost = x.LevelName })
                .ToList(),
        };
        return trait;
    }

    private async Task<BasicInfo> GetBasicInfo(GetCharacterSheetDataReportModel model)
    {
        var character = await characterRepository.GetCharacterInfoForCRB(model.CharacterId);
        var characterLevel = await xpRepository.GetCharacterXpLevel(model.CharacterId);
        var eventInfo = await eventCheckinRepository.GetActiveEventInfoOrDefaultAsync();
        var factionInfo = await characterFactionRepository.GetPlayerFactionInfo(model.CharacterId);
        var currentDay = 0;
        if (eventInfo is not null)
        {
            currentDay = await eventCheckinRepository.GetCurrentEventDay();
        }

        var basicInfo = new BasicInfo()
        {
            CharacterName = character.CharacterName,
            Expression = character.Expression,
            ProgressionPath = string.IsNullOrWhiteSpace(character.SecondaryProgressionName)
                ? character.PrimaryProgressionName ?? "None"
                : $"{character.PrimaryProgressionName} Secondary {character.SecondaryProgressionName}",
            PlayerNumber = character.PlayerNumber.ToString("D3"),
            PlayerName = character.PlayerName,
            LookupId = character.LookupId,
            CharacterLevel = characterLevel.ToString(),
            EventName = eventInfo?.Name ?? "No Active Event During Print",
            CurrentDay = currentDay,
            FactionName = factionInfo?.FactionName.Limit(21, ".") ?? "No Active Faction",
            FactionRank = factionInfo?.FactionRank.Limit(7, ".") ?? "-",
        };
        return basicInfo;
    }
}
