using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

namespace ExpressedRealms.DB.Models.Powers;

[AuditInclude]
public class Power : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public int LevelId { get; set; }
    public virtual PowerLevel PowerLevel { get; set; } = null!;

    public byte AreaOfEffectTypeId { get; set; }
    public virtual PowerAreaOfEffectType PowerAreaOfEffectType { get; set; } = null!;

    public byte ActivationTimingTypeId { get; set; }
    public virtual PowerActivationTimingType PowerActivationTimingType { get; set; } = null!;

    public byte DurationId { get; set; }
    public virtual PowerDuration PowerDuration { get; set; } = null!;

    public int PowerPathId { get; set; }
    public virtual PowerPath PowerPath { get; set; } = null!;

    public int? StatModifierGroupId { get; set; }
    public StatModifierGroup? StatModifierGroup { get; set; }

    public bool IsPowerUse { get; set; }
    public string? GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public string? OtherFields { get; set; }
    public string? Cost { get; set; }
    public int OrderIndex { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<PowerCategoryMapping> CategoryMappings { get; set; } = null!;

    public virtual PowerPrerequisite? Prerequisite { get; set; } = null!;
    public virtual List<PowerPrerequisitePower> PrerequisitePowers { get; set; } = null!;
    public virtual List<PowerAuditTrail> PowerAuditTrails { get; set; } = null!;
    public virtual List<CharacterPowerMapping> CharacterPowerMappings { get; set; } = null!;
}
