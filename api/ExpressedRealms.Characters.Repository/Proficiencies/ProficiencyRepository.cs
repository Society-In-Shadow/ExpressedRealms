using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.Data;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.Utilities;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Repositories.Shared;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Proficiencies;

internal sealed class ProficiencyRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    IStatModifierRepository statModifierRepository,
    IXpRepository xpRepository,
    CancellationToken cancellationToken
) : IProficiencyRepository
{
    public async Task<Result<List<ProficiencyDto>>> GetBasicProficiencies(int characterId)
    {
        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, characterId);

        var character = await query
            .Select(x => new { 
                x.Id, 
                x.ExpressionId, 
                x.PrimaryProgressionId,
                x.IsPrimaryCharacter,
                x.IsInCharacterCreation,
                x.Motes,
                x.WealthLevel
                }).FirstOrDefaultAsync();
        
        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        var availableModifiers = new List<ModifierDescription>();

        var stats = await context
            .CharacterStatMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.StatLevelId,
                x.StatTypeId,
            })
            .ToListAsync(cancellationToken);
        
        availableModifiers.AddRange(
            stats.Select(x =>
            {
                var modifierType = ModiferConversions.GetModifierType((StatType)x.StatTypeId);
                return new ModifierDescription()
                {
                    Value = x.StatLevelId,
                    Message = $"{modifierType.Name} Level",
                    Type = modifierType,
                    Name = modifierType.Name,
                };
            })
        );

        await FetchCharacterSkillsModifiers(characterId, availableModifiers);

        // All characters have this by default
        availableModifiers.Add(
            new ModifierDescription()
            {
                Value = 7,
                Message = "Standard",
                Type = ModifierType.Mortis,
                Name = "Mortis",
            }
        );
        
        availableModifiers.Add(
            new ModifierDescription()
            {
                Value = character.WealthLevel,
                Message = "Standard",
                Type = ModifierType.WealthLevel,
                Name = "Assigned Wealth Level",
            }
        );
        
        availableModifiers.Add(
            new ModifierDescription()
            {
                Value = character.Motes,
                Message = "Standard",
                Type = ModifierType.Motes,
                Name = "Assigned Prima / Void",
            }
        );

        var currentLevel = await xpRepository.GetCharacterXpLevel(characterId, character.IsPrimaryCharacter, character.IsInCharacterCreation);
        var extraModifiers = new List<ModifierDescription>();
        var dbModifiers = new List<ProficiencyModifierInfoDto>();

        dbModifiers.AddRange(await statModifierRepository.GetModifiersFromBlessings(characterId));
        dbModifiers.AddRange(await statModifierRepository.GetModifiersFromPowers(characterId));
        dbModifiers.AddRange(
            await statModifierRepository.GetModifiersFromXlLevel(currentLevel, character.ExpressionId, character.PrimaryProgressionId)
        );
        dbModifiers = dbModifiers
            .Where(x => x.TargetExpressionId == null || x.TargetExpressionId == character.ExpressionId)
            .ToList();

        extraModifiers.AddRange(
            dbModifiers.Select(x => new ModifierDescription()
            {
                Message = x.Source,
                Type = ModiferConversions.GetModifierType(x),
                Name = x.Source,
                Value = CalculatedValue(x),
            })
        );

        int CalculatedValue(ProficiencyModifierInfoDto modifier)
        {
            if (!modifier.ScaleWithLevel)
                return modifier.Modifier;

            if (modifier.CreationSpecificBonus && modifier.ScaleWithLevel)
                return modifier.Modifier + (modifier.Modifier * currentLevel);

            return modifier.Modifier * currentLevel;
        }

        var proficiencies = ProficiencyDtos.GetProficiencies(character.ExpressionId);

        foreach (var proficiency in proficiencies)
        {
            // Go through them one by one, as we have duplicates
            foreach (var modifier in proficiency.Modifiers)
            {
                proficiency.AppliedModifiers.AddRange(
                    availableModifiers.Where(x => x.Type == modifier).ToList()
                );
            }

            proficiency.AppliedModifiers.AddRange(
                extraModifiers.Where(x =>
                    x.Type.Name.Equals(
                        proficiency.Name,
                        StringComparison.InvariantCultureIgnoreCase
                    )
                )
            );
        }

        return proficiencies;
    }

    private async Task FetchCharacterSkillsModifiers(int characterId, List<ModifierDescription> availableModifiers)
    {
        var skills = await context
            .CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.SkillTypeId,
                LevelValue = x.SkillLevel.Level,
                SkillTypeName = x.SkillType.Name,
            })
            .ToListAsync(cancellationToken);

        availableModifiers.AddRange(
            skills.Select(x => new ModifierDescription()
            {
                Value = x.LevelValue,
                Message = $"{x.SkillTypeName} Level",
                Type = ModiferConversions.GetModifierType((SkillTypes)x.SkillTypeId),
                Name = ModiferConversions.GetModifierType((SkillTypes)x.SkillTypeId).Name,
            })
        );

        if (skills.Any(x => (SkillTypes)x.SkillTypeId == SkillTypes.Deflection && x.LevelValue == 4))
        {
            availableModifiers.Add(
                new ModifierDescription()
                {
                    Value = 1,
                    Message = "Skill Level Benefit",
                    Type = ModifierType.RWP,
                    Name = "Deflection Skill - Master Level",
                }
            );
        }
    }
}
