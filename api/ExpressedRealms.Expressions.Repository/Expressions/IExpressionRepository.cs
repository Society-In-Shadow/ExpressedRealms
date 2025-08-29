using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using FluentResults;

namespace ExpressedRealms.Expressions.Repository.Expressions;

public interface IExpressionRepository
{
    Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto);
    Task<Result<int>> EditExpressionAsync(EditExpressionDto dto);
    Task<Result> DeleteExpressionAsync(int id);
    Task<Result<List<ExpressionNavigationMenuItem>>> GetNavigationMenuItems(int expressionTypeId);
    Task<Result<GetExpressionDto>> GetExpression(int expressionId);
    Task<Expression?> GetExpressionForDeletion(int id);
    Task<Result<int>> GetCmsExpressionId(int id);
    Task<Expression?> ExpressionExists(int id);
    Task<bool> ExpressionTypeExists(int id);
}
