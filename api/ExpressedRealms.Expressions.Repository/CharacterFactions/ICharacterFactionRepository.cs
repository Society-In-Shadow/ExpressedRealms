using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;

namespace ExpressedRealms.Expressions.Repository.CharacterFactions;

public interface ICharacterFactionRepository
{
    Task<int> JoinFaction(CharacterFactionMapping characterFactionMapping);
}
