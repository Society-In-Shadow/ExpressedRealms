using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class ExpressionMenuItem(ExpressionNavigationMenuItem expressionNavigationMenuItem)
{
    public int Id { get; init; } = expressionNavigationMenuItem.Id;
    public string Name { get; init; } = expressionNavigationMenuItem.Name;
    public string ShortDescription { get; init; } = expressionNavigationMenuItem.ShortDescription;
    public string NavMenuImage { get; init; } = expressionNavigationMenuItem.NavMenuImage;
    public string? StatusName { get; init; } = expressionNavigationMenuItem.PublishStatusName;
    public int StatusId { get; init; } = (int)expressionNavigationMenuItem.PublishStatusId;
}
