namespace ExpressedRealms.Characters.Repository.Stats.DTOs;

public record GetAllStatsInfo()
{
    public bool ExtraMortis { get; init; }
    public List<SmallStatInfo> StatInfos { get; init; }
};
