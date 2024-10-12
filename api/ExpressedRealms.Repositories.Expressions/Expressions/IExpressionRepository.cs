using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Expressions.Expressions;

public interface IExpressionRepository
{
    Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto);
    Task<Result<int>> EditExpressionAsync(EditExpressionDto dto);
    Task<Result> DeleteExpressionAsync(int id);
    Task<Result<List<ExpressionNavigationMenuItem>>> GetNavigationMenuItems();
    Task<Result<GetExpressionDto>> GetExpression(int expressionId);
}
