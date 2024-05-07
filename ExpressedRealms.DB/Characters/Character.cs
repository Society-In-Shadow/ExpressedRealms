using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;

namespace ExpressedRealms.DB.Characters;

public class Character : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
    public Guid PlayerId { get; set; }
    public int ExpressionId { get; set; }

    public byte AgilityId { get; set; }
    public byte ConstitutionId { get; set; }
    public byte DexterityId { get; set; }
    public byte StrengthId { get; set; }
    public byte IntelligenceId { get; set; }
    public byte WillpowerId { get; set; }

    public int FactionId { get; set; }

    public int StatExperiencePoints { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Player Player { get; set; } = null!;
    public virtual Expression Expression { get; set; } = null!;

    public virtual StatLevel AgilityStatLevel { get; set; } = null!;
    public virtual StatLevel ConstitutionStatLevel { get; set; } = null!;
    public virtual StatLevel DexterityStatLevel { get; set; } = null!;
    public virtual StatLevel StrengthStatLevel { get; set; } = null!;
    public virtual StatLevel IntelligenceStatLevel { get; set; } = null!;
    public virtual StatLevel WillpowerStatLevel { get; set; } = null!;
    public virtual ExpressionSection FactionInfo { get; set; } = null!;
}
