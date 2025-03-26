using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Powers;

public class Power : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int LevelId { get; set; }
    public int AreaOfEffectTypeId { get; set; }
    public int ActivationTimingTypeId { get; set; }
    public int DurationId { get; set; }
    
    public bool IsPowerUse { get; set; }
    public string? GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public string? OtherFields { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public virtual PowerLevel PowerLevel { get; set; } = null!;
    public virtual PowerAreaOfEffectType PowerAreaOfEffectType { get; set; } = null!;
    public virtual PowerActivationTimingType PowerActivationTimingType { get; set; } = null!;
    public virtual PowerDuration PowerDuration { get; set; } = null!;
}