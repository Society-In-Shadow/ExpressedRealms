using ExpressedRealms.Repositories.Expressions.Expressions;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;

public class EditExpressionRequest
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public PublishTypes PublishStatus { get; set; }
}
