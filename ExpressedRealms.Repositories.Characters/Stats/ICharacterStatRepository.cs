using FluentResults;

namespace ExpressedRealms.Repositories.Characters.Stats;

public interface ICharacterStatRepository
{
    Task<Result<SingleStatInfo>> GetDetailedStatInfo(GetDetailedStatInfoDto dto);
}