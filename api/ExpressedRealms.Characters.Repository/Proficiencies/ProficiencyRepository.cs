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
                x.SkillLevel.Level,
                x.SkillLevel.SkillLevelBenefits,
                x.SkillType
            })
            .ToListAsync(cancellationToken);

        availableModifiers.AddRange( skills.Select(x => new ModifierDescription()
        {
            Value = x.Level,
            Message = $"Base Skill for {x.SkillType.Name}",
            Type = GetModifierType((SkillTypes)x.SkillTypeId)
        }));

        availableModifiers.AddRange( skills
            .SelectMany(x => x.SkillLevelBenefits
                .Select(y => new ModifierDescription()
                    {
                        Value = y.Modifier,
                        Message = $"Skill Level Benefit for {x.SkillType.Name}",
                        Type = GetModifierType((DbModifierTypes)x.SkillTypeId)
                    }
                )
            ).Distinct()
        );
        
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
                    ModifierType.HandToHandOffense,
                    ModifierType.Strike
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
                    ModifierType.HandToHandDefense,
                    ModifierType.Dodge
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
                    ModifierType.MeleeOffense,
                    ModifierType.Thrust
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
                    ModifierType.MeleeDefense,
                    ModifierType.Parry
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
    private static ModifierType GetModifierType(DbModifierTypes statType)
    {
        return statType switch
        {
            DbModifierTypes.StrengthStonePull => ModifierType.Strength,
            DbModifierTypes.StrikeProficiencyModifier => ModifierType.Strike,
            DbModifierTypes.ConstitutionStonePull => ModifierType.Constitution,
            //DbModifierTypes.WisdomStonePull => ,
            DbModifierTypes.IntelligenceStonePull => ModifierType.Intelligence,
            DbModifierTypes.WillpowerStonePull => ModifierType.Willpower,
            DbModifierTypes.DexterityStonePull => ModifierType.Dexterity,
            DbModifierTypes.ThrustProficiencyModifier => ModifierType.Thrust,
            DbModifierTypes.ShootProficiencyModifier => ModifierType.Shoot,
            DbModifierTypes.ThrowRange => ModifierType.Throw,
            DbModifierTypes.CastProficiencyModifier => ModifierType.Cast,
            DbModifierTypes.DodgeGeneralDamageProficiency => ModifierType.Dodge,
            DbModifierTypes.ParryGeneralDamageProficiency => ModifierType.Parry,
            DbModifierTypes.EvadeGeneralDamageProficiency => ModifierType.Evade,
            DbModifierTypes.WardGeneralDamageProficiency => ModifierType.Ward,
            DbModifierTypes.DeflectGeneralDamageProficiency => ModifierType.Deflection,
            //DbModifierTypes.ReserveWillPower => expr,
            DbModifierTypes.ThrowStat => ModifierType.Throw,
            DbModifierTypes.ProjectStat => ModifierType.Project,
            DbModifierTypes.AgilityStonePull => ModifierType.Agility,
            DbModifierTypes.Project => ModifierType.Project,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}