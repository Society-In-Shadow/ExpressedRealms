using ExpressedRealms.Repositories.Characters.Stats.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Characters.Stats;

public interface ICharacterStatRepository
{
    Task<Result<SingleStatInfo>> GetDetailedStatInfo(GetDetailedStatInfoDto dto);
    Task<Result> UpdateCharacterStat(EditStatDto dto);
    Task<Result<List<SmallStatInfo>>> GetAllStats(int characterId);
}
