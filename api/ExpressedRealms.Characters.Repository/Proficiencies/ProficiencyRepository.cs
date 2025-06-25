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

        var skills = await context.CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.SkillTypeId,
                x.SkillLevel.Level
            })
            .ToListAsync(cancellationToken);


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
        };

        foreach (var proficiency in proficiencies)
        {
            foreach (var modifierType in proficiency.Modifiers)
            {
                switch (modifierType)
                {
                    case ModifierType.Dexterity:
                        proficiency.AppliedModifiers.Add(new ModifierDescription()
                        {
                            Message = "Base Dexterity Stat",
                            Type = modifierType,
                            Value = stats.Value.First(x => x.StatTypeId == StatType.Dexterity).Bonus,
                        });
                        break;
                    case ModifierType.Strength:
                        proficiency.AppliedModifiers.Add(new ModifierDescription()
                        {
                            Message = "Base Strength Stat",
                            Type = modifierType,
                            Value = stats.Value.First(x => x.StatTypeId == StatType.Strength).Bonus,
                        });
                        break;
                    case ModifierType.Agility:
                        proficiency.AppliedModifiers.Add(new ModifierDescription()
                        {
                            Message = "Base Agility Stat",
                            Type = modifierType,
                            Value = stats.Value.First(x => x.StatTypeId == StatType.Agility).Bonus,
                        });
                        break;
                    case ModifierType.HandToHandOffense:
                        proficiency.AppliedModifiers.Add(new ModifierDescription()
                        {
                            Message = "Base Hand To Hand Offense Stat",
                            Type = modifierType,
                            Value = skills.First(x => x.SkillTypeId == (int)SkillTypes.HandToHandOffense).Level,
                        });
                        break;
                    case ModifierType.HandToHandDefense:
                        proficiency.AppliedModifiers.Add(new ModifierDescription()
                        {
                            Message = "Base Hand To Hand Defense Stat",
                            Type = modifierType,
                            Value = skills.First(x => x.SkillTypeId == (int)SkillTypes.HandToHandDefense).Level,
                        });
                        break;

                }
            }
        }
        
        return proficiencies;
    }
}