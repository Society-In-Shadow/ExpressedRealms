using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.StatModifier;

public class StatModifierRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IStatModifierRepository
{
    public async Task<StatGroupMapping> GetGroupMappingForEditing(int id)
    {
        return await context.StatGroupMappings.FirstAsync(x => x.Id == id);
    }

    public async Task<List<StatGroupMapping>> GetGroupMappings(int groupId)
    {
        return await context
            .StatGroupMappings.AsNoTracking()
            .Where(x => x.StatGroupId == groupId)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<DB.Models.ModifierSystem.StatModifiers.StatModifier>> GetModifierTypes()
    {
        return await context.StatModifiers.AsNoTracking().ToListAsync();
    }

    public async Task<bool> ProgressionLevelExists(int id)
    {
        return await context.ProgressionLevel.AnyAsync(x => x.Id == id);
    }

    public async Task<List<ProficiencyModifierInfoDto>> GetModifiersFromBlessings(int characterId)
    {
        return await context
            .CharacterBlessingMappings.Where(x =>
                x.CharacterId == characterId && x.BlessingLevel.StatModifierGroup != null
            )
            .SelectMany(x =>
                x.BlessingLevel.StatModifierGroup!.StatGroupMappings.Select(
                    y => new ProficiencyModifierInfoDto
                    {
                        Source = $"{x.Blessing.Name} {x.Blessing.Type}",
                        Modifier = y.Modifier,
                        ModifierTypeId = y.StatModifierId,
                        ScaleWithLevel = y.ScaleWithLevel,
                        CreationSpecificBonus = y.CreationSpecificBonus,
                        TargetExpressionId = y.TargetExpressionId,
                    }
                )
            )
            .ToListAsync();
    }

    public async Task<List<ProficiencyModifierInfoDto>> GetModifiersFromPowers(int characterId)
    {
        return await context
            .CharacterPowerMappings.Where(x =>
                x.CharacterId == characterId && x.Power.StatModifierGroup != null
            )
            .SelectMany(x =>
                x.Power.StatModifierGroup!.StatGroupMappings.Select(
                    y => new ProficiencyModifierInfoDto
                    {
                        Source = $"{x.Power.Name} Power",
                        Modifier = y.Modifier,
                        ModifierTypeId = y.StatModifierId,
                        ScaleWithLevel = y.ScaleWithLevel,
                        CreationSpecificBonus = y.CreationSpecificBonus,
                        TargetExpressionId = y.TargetExpressionId,
                    }
                )
            )
            .ToListAsync();
    }

    public async Task<List<ProficiencyModifierInfoDto>> GetModifiersFromXlLevel(
        int characterId,
        int currentLevel
    )
    {
        var character = await context.Characters.Where(x => x.Id == characterId).FirstAsync();

        var progressionExpression = context.ProgressionLevel.Where(x =>
            x.ProgressionPath.ExpressionId == character.ExpressionId
            && x.StatModifierGroup != null
            && x.XlLevel <= currentLevel
        );

        // Sorcerer, Adept, Vampyre, Shammas
        if (character.PrimaryProgressionId.HasValue)
        {
            progressionExpression = progressionExpression.Where(x =>
                x.ProgressionPathId == character.PrimaryProgressionId
            );
        }
        else
        {
            var availableProgressions = await context.ProgressionPath.FirstAsync(x =>
                x.ExpressionId == character.ExpressionId
            );
            progressionExpression = progressionExpression.Where(x =>
                x.ProgressionPathId == availableProgressions.Id
            );
        }

        return await progressionExpression
            .SelectMany(x =>
                x.StatModifierGroup!.StatGroupMappings.Select(y => new ProficiencyModifierInfoDto
                {
                    Source = $"XL {x.XlLevel}",
                    Modifier = y.Modifier,
                    ModifierTypeId = y.StatModifierId,
                    ScaleWithLevel = y.ScaleWithLevel,
                    CreationSpecificBonus = y.CreationSpecificBonus,
                    TargetExpressionId = y.TargetExpressionId,
                })
            )
            .ToListAsync();
    }

    public async Task<bool> ModifierTypeExists(int id)
    {
        return await context.StatModifiers.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> GroupIdExists(int id)
    {
        return await context.StatModifierGroups.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ProgressionPathExists(int id)
    {
        return await context.ProgressionPath.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> PowerExists(int id)
    {
        return await context.Powers.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> BlessingLevelExists(int id)
    {
        return await context.BlessingLevels.AnyAsync(x => x.Id == id);
    }

    public async Task UpdateBlessingGroupId(int blessingId, int groupId)
    {
        var blessing = await context.BlessingLevels.FirstAsync(x => x.Id == blessingId);
        blessing.StatModifierGroupId = groupId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProgressionPathGroupId(int progressionPathId, int groupId)
    {
        var progressionPath = await context.ProgressionLevel.FirstAsync(x =>
            x.Id == progressionPathId
        );
        progressionPath.StatModifierGroupId = groupId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePowerGroupId(int powerId, int groupId)
    {
        var power = await context.Powers.FirstAsync(x => x.Id == powerId);
        power.StatModifierGroupId = groupId;
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateGroupMapping(StatGroupMapping mapping)
    {
        context.StatGroupMappings.Update(mapping);
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AddGroup()
    {
        var group = new StatModifierGroup();
        context.StatModifierGroups.Add(group);
        await context.SaveChangesAsync(cancellationToken);
        return group.Id;
    }

    public async Task<int> AddStatGroupMapping(StatGroupMapping mapping)
    {
        context.StatGroupMappings.Add(mapping);
        await context.SaveChangesAsync(cancellationToken);
        return mapping.Id;
    }

    public Task HardDeleteGroupMapping(StatGroupMapping mapping)
    {
        context.StatGroupMappings.Remove(mapping);
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> GroupMappingExists(int groupId, int id)
    {
        return await context.StatGroupMappings.AnyAsync(x =>
            x.StatGroupId == groupId && x.Id == id
        );
    }
}
