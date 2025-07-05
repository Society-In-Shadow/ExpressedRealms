using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using FluentResults;

namespace ExpressedRealms.Expressions.Repository.Expressions;

public interface IExpressionRepository
{
    Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto);
    Task<Result<int>> EditExpressionAsync(EditExpressionDto dto);
    Task<Result> DeleteExpressionAsync(int id);
    Task<Result<List<ExpressionNavigationMenuItem>>> GetNavigationMenuItems();
    Task<Result<GetExpressionDto>> GetExpression(int expressionId);
    Task<Result<int>> GetGameSystemExpressionId();
    Task<Result<int>> GetTreasuredTalesExpressionId();
}
