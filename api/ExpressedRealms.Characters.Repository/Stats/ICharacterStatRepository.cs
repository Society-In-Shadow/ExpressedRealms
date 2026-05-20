using ExpressedRealms.Characters.Repository.Stats.DTOs;
using FluentResults;

namespace ExpressedRealms.Characters.Repository.Stats;

public interface ICharacterStatRepository
{
    Task<Result<SingleStatInfo>> GetDetailedStatInfo(GetDetailedStatInfoDto dto);
    Task<Result> UpdateCharacterStat(EditStatDto dto);
    Task<Result<List<SmallStatInfo>>> GetAllStats(int characterId);
}
