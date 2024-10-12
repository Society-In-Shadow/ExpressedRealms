using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;

public class EditExpressionResponse(GetExpressionDto dto)
{
    public int Id { get; init; } = dto.Id;
    public string Name { get; set; } = dto.Name;
    public string ShortDescription { get; set; } = dto.ShortDescription;
    public string NavMenuImage { get; set; } = dto.NavMenuImage;
    public PublishTypes PublishStatus { get; set; } = dto.PublishStatus;
    public List<KeyValuePair<int, string>> PublishTypes { get; set; } = dto.PublishTypes;
}