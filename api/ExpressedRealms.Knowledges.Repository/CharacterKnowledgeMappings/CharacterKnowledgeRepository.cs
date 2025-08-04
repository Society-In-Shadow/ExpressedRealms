using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;

public class CharacterKnowledgeRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ICharacterKnowledgeRepository
{
    public async Task<int> GetExperienceSpentOnKnowledgesForCharacter(int characterId)
    {
        const int unknownKnowledgeType = 3;
        return await context
            .CharacterKnowledgeMappings.Where(x => x.CharacterId == characterId)
            .SumAsync(
                x =>
                    x.Knowledge.KnowledgeTypeId == unknownKnowledgeType
                        ? x.KnowledgeLevel.TotalUnknownXpCost
                        : x.KnowledgeLevel.TotalGeneralXpCost,
                cancellationToken
            );
    }

    public async Task<int> AddCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping)
    {
        context.Add(mapping);
        await context.SaveChangesAsync(cancellationToken);
        return mapping.Id;
    }

    public async Task<bool> MappingAlreadyExists(int knowledgeId, int characterId)
    {
        return await context
            .CharacterKnowledgeMappings.AsNoTracking()
            .AnyAsync(
                x => x.KnowledgeId == knowledgeId && x.CharacterId == characterId,
                cancellationToken
            );
    }

    public Task<bool> MappingAlreadyExists(int mappingId)
    {
        return context
            .CharacterKnowledgeMappings.AsNoTracking()
            .AnyAsync(x => x.Id == mappingId, cancellationToken);
    }

    public Task<CharacterKnowledgeMapping> GetCharacterKnowledgeMappingForEditing(
        int modelMappingId
    )
    {
        return context.CharacterKnowledgeMappings.FirstAsync(
            x => x.Id == modelMappingId,
            cancellationToken
        );
    }

    public async Task UpdateCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping)
    {
        context.Update(mapping);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CharacterKnowledgeProjection>> GetKnowledgesForCharacter(int characterId)
    {
        return await context
            .CharacterKnowledgeMappings.Where(x => x.CharacterId == characterId)
            .Select(x => new CharacterKnowledgeProjection()
            {
                MappingId = x.Id,
                Knowledge = new KnowledgeProjection()
                {
                    Name = x.Knowledge.Name,
                    Description = x.Knowledge.Description,
                    Type = x.Knowledge.KnowledgeType.Name,
                },
                StoneModifier = x.KnowledgeLevel.StoneModifier,
                LevelName = x.KnowledgeLevel.Name,
                Level = x.KnowledgeLevel.Level,
                Specializations = x
                    .CharacterKnowledgeSpecializations.Select(y => new SpecializationProjection()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                    })
                    .ToList(),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<SpecializationCountProjection> GetSpecializationCountForMapping(int mappingId)
    {
        return await context.CharacterKnowledgeMappings.Where(x => x.Id == mappingId)
            .Select(x => new SpecializationCountProjection()
            {
                MaxCount = x.KnowledgeLevel.SpecializationCount,
                CurrentCount = x.CharacterKnowledgeSpecializations.Count,
            })
            .FirstAsync(cancellationToken);
    }
    
    public async Task<bool> HasExistingSpecializationForMapping(int mappingId, string name)
    {
        return await context
            .CharacterKnowledgeMappings.AsNoTracking()
            .AnyAsync(
                x => x.Id == mappingId && 
                     x.CharacterKnowledgeSpecializations.Any(y => y.Name.ToLower() == name.ToLower()),
                cancellationToken
            );
    }
}
