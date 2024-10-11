using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Expressions.Expressions;

internal sealed class ExpressionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionDtoValidator createExpressionDtoValidator,
    EditExpressionDtoValidator editExpressionDtoValidator,
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
    
    public async Task<Result<int>> EditExpressionAsync(EditExpressionDto dto)
    {
        var result = await editExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var expression = await context.Expressions
            .Where(x => x.Id == dto.Id)
            .FirstOrDefaultAsync();
        
        if (expression is null)
            return Result.Fail(new NotFoundFailure("Expression"));
        
        expression.Name = dto.Name;
        expression.ShortDescription = dto.ShortDescription;
        expression.NavMenuImage = dto.NavMenuImage;
        
        context.Expressions.Update(expression);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(expression.Id);
    }
}