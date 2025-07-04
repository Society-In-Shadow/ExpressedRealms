using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.Data;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.Utilities;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.DB;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Proficiencies;

internal sealed class ProficiencyRepository(
    ExpressedRealmsDbContext context,
    ICharacterStatRepository characterStatRepository,
    CancellationToken cancellationToken
) : IProficiencyRepository
{
    public async Task<Result<List<ProficiencyDto>>> GetBasicProficiencies(int characterId)
    {
        var stats = await characterStatRepository.GetAllStats(characterId);

        if (stats.IsFailed)
        {
            return Result.Fail(stats.Errors);
        }

        var availableModifiers = new List<ModifierDescription>();

        availableModifiers.AddRange(
            stats.Value.Select(x => new ModifierDescription()
            {
                Value = x.Level,
                Message = $"{ModiferConversions.GetModifierType(x.StatTypeId).Name} Level",
                Type = ModiferConversions.GetModifierType(x.StatTypeId),
                Name = ModiferConversions.GetModifierType(x.StatTypeId).Name,
            })
        );

        var skills = await context
            .CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.SkillTypeId,
                LevelValue = x.SkillLevel.Level,
                Benefits = x
                    .SkillLevel.SkillLevelBenefits.Where(y => y.SkillTypeId == x.SkillTypeId)
                    .Select(z => new { z.Modifier, z.ModifierTypeId })
                    .ToList(),
                SkillTypeName = x.SkillType.Name,
                SkillLevelName = x.SkillLevel.Name,
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

        // All characters have this by default
        availableModifiers.Add(
            new ModifierDescription()
            {
                Value = 8,
                Message = "Standard",
                Type = ModifierType.Mortis,
                Name = "Mortis",
            }
        );

        var proficiencies = ProficiencyDtos.GetProficiencies();

        foreach (var proficiency in proficiencies)
        {
            // Go through them one by one, as we have duplicates
            foreach (var modifier in proficiency.Modifiers)
            {
                proficiency.AppliedModifiers.AddRange(
                    availableModifiers.Where(x => x.Type == modifier).ToList()
                );
            }
        }

        return proficiencies;
    }
}
