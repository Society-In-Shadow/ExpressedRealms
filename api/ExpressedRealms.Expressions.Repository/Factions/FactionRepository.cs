using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions.CmsTypeSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionPublishStatusSetup;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;
using ExpressedRealms.Expressions.Repository.Factions.Dtos.ExpressionFactionDtos;
using Microsoft.EntityFrameworkCore;
using FactionDto = ExpressedRealms.Expressions.Repository.Factions.Dtos.FactionDto;

namespace ExpressedRealms.Expressions.Repository.Factions;

internal sealed class FactionRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IFactionRepository
{
    public async Task<int> CreateFactionAsync(Faction faction)
    {
        context.Factions.Add(faction);
        await context.SaveChangesAsync(cancellationToken);
        return faction.Id;
    }

    public async Task<bool> HasDuplicateName(string name, int factionId = 0)
    {
        if (factionId != 0)
        {
            return await context
                .Factions.AsNoTracking()
                .AnyAsync(
                    x => x.Name.ToLower() == name.ToLower() && x.Id != factionId,
                    cancellationToken
                );
        }
        return await context
            .Factions.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task EditFactionAsync(Faction faction)
    {
        context.Factions.Update(faction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Faction?> GetFactionForEditingAsync(int id)
    {
        return context.Factions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<int?> GetBasicFactionRankId(int id, int expressionId)
    {
        return context
            .FactionLevels.Where(x =>
                x.FactionId == id
                && x.Faction.ExpressionId == expressionId
                && x.FactionRank.Id == FactionRankEnum.Basic.Value
            )
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<FactionLevel?> GetFactionLevelAsync(int factionLevelId)
    {
        return context.FactionLevels.FirstOrDefaultAsync(
            x => x.Id == factionLevelId,
            cancellationToken
        );
    }

    public async Task<List<FactionDto>> GetFactions(int expressionId)
    {
        return await context
            .Factions.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new FactionDto()
            {
                Id = x.Id,
                Name = x.Name,
                Background = x.Background,
                ExpressionId = x.ExpressionId,
                Levels = x
                    .FactionLevels.Select(y => new FactionLevelListDto()
                    {
                        Id = y.Id,
                        RankName = y.FactionRank.Name,
                        KnowledgeId = y.KnowledgeId,
                        Knowledge = y.KnowledgeId == null ? null : y.Knowledge!.Name,
                        KnowledgeLevel =
                            y.KnowledgeLevelId == null
                                ? null
                                : $"{y.KnowledgeLevel!.Name} ({y.KnowledgeLevel.Level})",
                        KnowledgeLevelId = y.KnowledgeLevelId,
                        Specialization = y.Specialization,
                        PowerId = y.PowerId,
                    })
                    .ToList(),
            })
            .Where(x => x.ExpressionId == expressionId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<ExpressionDto>> GetExpressionFactions()
    {
        List<int> availableExpressions = [ExpressionPublishStatusEnum.Published.Value, ExpressionPublishStatusEnum.PlayTesting.Value];
        return await context.Expressions
            .Where(x => availableExpressions.Contains(x.PublishStatusId) && x.CmsTypeId == CmsTypeEnum.Expression)
            .OrderBy(x => x.Name)
            .Select(x => new ExpressionDto()
            {
                Id = x.Id,
                Name = x.Name,
                Factions = x.Factions
                    .OrderBy(x => x.Name)
                    .Select(y => new ExpressionFactionDto()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Players = y.FactionLevels.OrderBy(x => x.FactionRankId).SelectMany(z => z.CharacterFactionMappings
                            .Where(x => x.Character.IsPrimaryCharacter)
                            .Select(a => new PlayerDto()
                            {
                                Id = a.Character.Id,
                                CharacterName = a.Character.Name,
                                Level = z.FactionRank.Id,
                                LevelName = z.FactionRank.Name,
                                Player = $"{a.Character.Player.Name} ({a.Character.Player.PlayerNumber})"
                            }).ToList()
                        ).ToList()
                    }).ToList()
                }).ToListAsync(cancellationToken);
    }

    public Task<Faction?> GetFactionAsync(int id)
    {
        return context
            .Factions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
