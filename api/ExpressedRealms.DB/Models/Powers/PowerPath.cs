using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerPath : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public int ExpressionId { get; set; }
    public virtual Expression Expression { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<Power> Powers { get; set; } = null!;
}
