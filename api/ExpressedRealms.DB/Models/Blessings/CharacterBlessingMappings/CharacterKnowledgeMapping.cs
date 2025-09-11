using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;

public class CharacterBlessingMapping : ISoftDelete
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int BlessingId { get; set; }
    public int BlessingLevelId { get; set; }
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Blessing Blessing { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual BlessingLevel BlessingLevel { get; set; } = null!;
}
