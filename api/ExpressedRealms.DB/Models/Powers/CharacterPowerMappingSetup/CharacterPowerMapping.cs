using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters;

namespace ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;

public class CharacterPowerMapping : ISoftDelete
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int PowerId { get; set; }
    public int PowerLevelId { get; set; }
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Power Power { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual PowerLevel PowerLevel { get; set; } = null!;
}
