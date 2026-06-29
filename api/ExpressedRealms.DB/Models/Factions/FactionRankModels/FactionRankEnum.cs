using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Factions.FactionRankModels;

public sealed class FactionRankEnum : SmartEnum<FactionRankEnum, int>
{
    private FactionRankEnum(string name, int id)
        : base(name, id) { }

    public static readonly FactionRankEnum Basic = new("Basic", 1);

    public static readonly FactionRankEnum Intermediate = new("Intermediate", 2);

    public static readonly FactionRankEnum Advance = new("Advance", 3);

    public static readonly FactionRankEnum Supreme = new("Supreme", 4);
}
