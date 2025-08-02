using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
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
            .SumAsync(x =>
                x.Knowledge.KnowledgeTypeId == unknownKnowledgeType
                    ? x.KnowledgeLevel.UnknownXpCost
                    : x.KnowledgeLevel.GeneralXpCost,
                cancellationToken
            );
    }
    
    public async Task<int> GetExperienceSpentOnKnowledgesForCharacterSansCurrentKnowledge(int characterId, int knowledgeId)
    {
        const int unknownKnowledgeType = 3;
        return await context
            .CharacterKnowledgeMappings.Where(x => x.CharacterId == characterId && x.KnowledgeId != knowledgeId)
            .SumAsync(x =>
                    x.Knowledge.KnowledgeTypeId == unknownKnowledgeType
                        ? x.KnowledgeLevel.UnknownXpCost
                        : x.KnowledgeLevel.GeneralXpCost,
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

    public Task<CharacterKnowledgeMapping> GetCharacterKnowledgeMappingForEditing(int modelMappingId)
    {
        return context.CharacterKnowledgeMappings.FirstAsync(x => x.Id == modelMappingId, cancellationToken);
    }
    
    public async Task UpdateCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping)
    {
        context.Update(mapping);
        await context.SaveChangesAsync(cancellationToken);
    }
}
