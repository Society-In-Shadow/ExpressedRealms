using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;

namespace ExpressedRealms.Expressions.Repository.CharacterFactions;

internal sealed class CharacterFactionRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ICharacterFactionRepository
{
    public async Task<int> JoinFaction(CharacterFactionMapping characterFactionMapping)
    {
        await context.CharacterFactionMappings.AddAsync(characterFactionMapping, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return characterFactionMapping.Id;
    }
}
