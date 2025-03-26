using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

namespace ExpressedRealms.DB.Models.Powers;

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
    
    public int ExpressionId { get; set; }
    public virtual Expression Expression { get; set; } = null!;
    
    public bool IsPowerUse { get; set; }
    public string? GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public string? OtherFields { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public virtual List<PowerCategoryMapping> CategoryMappings { get; set; } = null!;
    
    public virtual List<PowerPrerequisites> PrerequisitePowers { get; set; } = null!;
}