using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;

namespace ExpressedRealms.Expressions.Repository.FactionLevels;

public interface IFactionLevelRepository
{
    Task CreateFactionLevelsAsync(List<FactionLevel> factionLevels);
    Task<List<FactionLevelListDto>> GetFactionLevels();
    Task<FactionLevel?> GetFactionLevelAsync(int id);
}
