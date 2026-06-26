using ExpressedRealms.DB.Models.Factions.FactionModels;

namespace ExpressedRealms.Expressions.Repository.Factions;

public interface IFactionRepository
{
    Task<int> CreateFactionAsync(Faction faction);
    Task<bool> HasDuplicateName(string name, int factionId = 0);
    Task EditFactionAsync(Faction faction);
    Task<Faction?> GetFactionForEditingAsync(int id);
    Task<List<Faction>> GetFactions(int expressionId);
    Task<Faction?> GetFactionAsync(int id);
}
