using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<Expression> Expressions { get; set; } = null!;
}
