using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;

namespace ExpressedRealms.DB.Characters;

public class Character : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
    public Guid PlayerId { get; set; }
    public int ExpressionId { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Player Player { get; set; } = null!;
    public virtual Expression Expression { get; set; } = null!;
}
