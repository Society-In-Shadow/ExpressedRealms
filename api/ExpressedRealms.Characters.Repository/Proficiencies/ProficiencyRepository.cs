using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.DB;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using StatType = ExpressedRealms.Characters.Repository.Stats.Enums.StatType;

namespace ExpressedRealms.Characters.Repository.Proficiencies;

internal sealed class ProficiencyRepository (ExpressedRealmsDbContext context, 
    ICharacterStatRepository characterStatRepository,
    CancellationToken cancellationToken) : IProficiencyRepository
{
    public async Task<Result<List<ProficiencyDto>>> GetBasicProficiencies(int characterId)
    {
        var stats = await characterStatRepository.GetAllStats(characterId);

        if (stats.IsFailed)
        {
            return Result.Fail(stats.Errors);
        }

        var availableModifiers = new List<ModifierDescription>();
        
        availableModifiers.AddRange( stats.Value.Select(x => new ModifierDescription()
        {
            Value = x.Bonus,
            Message = "Base Stat",
            Type = GetModifierType(x.StatTypeId)
        }));
        
        var skills = await context.CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.SkillTypeId,
                x.SkillLevel.Level
            })
            .ToListAsync(cancellationToken);

        availableModifiers.AddRange( skills.Select(x => new ModifierDescription()
        {
            Value = x.Level,
            Message = "Base Skill",
            Type = GetModifierType((SkillTypes)x.SkillTypeId)
        }));

        var proficiencies = new List<ProficiencyDto>()
        {
            new ProficiencyDto()
            {
                Name = "Strike",
                Description = "Lorem Ipsum",
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Dexterity,
                    ModifierType.Strength,
                    ModifierType.HandToHandOffense
                },
                CorrespondingId = 1,
            },
            new ProficiencyDto()
            {
                Name = "Dodge",
                Description = "Lorem Ipsum",
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Strength,
                    ModifierType.HandToHandDefense
                },
                CorrespondingId = 1,
            },
            new ProficiencyDto()
            {
                Name = "Thrust",
                Description = "Lorem Ipsum",
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Dexterity,
                    ModifierType.MeleeOffense
                },
                CorrespondingId = 2,
            },
            new ProficiencyDto()
            {
                Name = "Parry",
                Description = "Lorem Ipsum",
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Strength,
                    ModifierType.MeleeDefense
                },
                CorrespondingId = 2,
            },
        };

        foreach (var proficiency in proficiencies)
        {
            proficiency.AppliedModifiers = availableModifiers.Where(x => proficiency.Modifiers.Contains(x.Type)).ToList();
        }
        
        return proficiencies;
    }

    private static ModifierType GetModifierType(StatType statType)
    {
        return statType switch
        {
            StatType.Dexterity => ModifierType.Dexterity,
            StatType.Strength => ModifierType.Strength,
            StatType.Agility => ModifierType.Agility,
            StatType.Intelligence => ModifierType.Intelligence,
            StatType.Willpower => ModifierType.Willpower,
            StatType.Constitution => ModifierType.Constitution,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
    
    private static ModifierType GetModifierType(SkillTypes statType)
    {
        return statType switch
        {
            SkillTypes.HandToHandDefense => ModifierType.HandToHandDefense,
            SkillTypes.HandToHandOffense => ModifierType.HandToHandOffense,
            SkillTypes.MeleeOffense => ModifierType.MeleeOffense,
            SkillTypes.Marksmanship => ModifierType.Marksmanship,
            SkillTypes.ThrownWeapons => ModifierType.ThrownWeapons,
            SkillTypes.Spellcasting => ModifierType.Spellcasting,
            SkillTypes.Projection => ModifierType.Projection,
            SkillTypes.MeleeDefense => ModifierType.MeleeDefense,
            SkillTypes.Acrobatics => ModifierType.Acrobatics,
            SkillTypes.Spellwarding => ModifierType.Spellwarding,
            SkillTypes.Deflection => ModifierType.Deflection,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}