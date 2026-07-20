using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;

namespace ExpressedRealms.Expressions.Repository.Factions;

public interface IFactionRepository
{
    Task<int> CreateFactionAsync(Faction faction);
    Task<bool> HasDuplicateName(string name, int factionId = 0);
    Task EditFactionAsync(Faction faction);
    Task<Faction?> GetFactionForEditingAsync(int id);
    Task<List<FactionDto>> GetFactions(int expressionId);
    Task<Faction?> GetFactionAsync(int id);
    Task<int?> GetBasicFactionRankId(int id, int expressionId);
}
