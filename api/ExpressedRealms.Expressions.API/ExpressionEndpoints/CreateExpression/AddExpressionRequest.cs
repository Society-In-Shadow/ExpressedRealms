namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.CreateExpression;

public class AddExpressionRequest
{
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
}
