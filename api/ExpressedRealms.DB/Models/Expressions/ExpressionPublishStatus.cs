namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionPublishStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<Expression> Expressions { get; set; } = null!;
}
