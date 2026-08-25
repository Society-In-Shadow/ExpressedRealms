namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetStatsForCharacter;

public record GetAllStatsResponse()
{
    public bool HasExtraMortis { get; init; }
    public List<SmallStatInfo> Stats { get; init; }
};
