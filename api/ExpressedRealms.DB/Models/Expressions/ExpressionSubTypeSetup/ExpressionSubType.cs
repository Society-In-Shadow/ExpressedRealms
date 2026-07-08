using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSubTypeSetup;

public class ExpressionSubType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual List<Expression> Expressions { get; set; } = null!;
}
