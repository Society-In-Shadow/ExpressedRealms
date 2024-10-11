using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Repositories.Expressions.Expressions;

internal sealed class ExpressionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionDtoValidator createExpressionDtoValidator,
    CancellationToken cancellationToken
) : IExpressionRepository
{
    public async Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto)
    {
        var result = await createExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var expression = new Expression()
        {
            Name = dto.Name,
            ShortDescription = dto.ShortDescription,
            NavMenuImage = dto.NavMenuImage
        };
        
        context.Expressions.Add(expression);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(expression.Id);
    }
}