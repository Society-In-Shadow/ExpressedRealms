using ExpressedRealms.Authentication;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Repositories.Shared.Helpers;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.Expressions;

internal sealed class ExpressionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionDtoValidator createExpressionDtoValidator,
    EditExpressionDtoValidator editExpressionDtoValidator,
    IUserContext userContext,
    CancellationToken cancellationToken
) : IExpressionRepository
{
    public async Task<Result<List<ExpressionNavigationMenuItem>>> GetNavigationMenuItems()
    {
        var canSeeBetaAndDrafts = await userContext.CurrentUserHasPolicy(
            Policies.ExpressionEditorPolicy
        );

        var expression = context.Expressions.AsNoTracking().Where(x => x.ExpressionTypeId == 1); // 1 = expression

        if (!canSeeBetaAndDrafts)
        {
            expression = expression.Where(e => e.PublishStatusId == (int)PublishTypes.Published);
        }

        return await expression
            .Select(x => new ExpressionNavigationMenuItem()
            {
                Name = x.Name,
                Id = x.Id,
                ShortDescription = x.ShortDescription,
                NavMenuImage = x.NavMenuImage,
                PublishStatusName = x.PublishStatus.Name,
                PublishStatusId = (PublishTypes)x.PublishStatusId,
            })
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<int>> GetGameSystemExpressionId()
    {
        return await context
            .Expressions.Where(x => x.ExpressionTypeId == 2)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Result<int>> GetTreasuredTalesExpressionId()
    {
        return await context
            .Expressions.Where(x => x.ExpressionTypeId == 3)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Result<GetExpressionDto>> GetExpression(int expressionId)
    {
        var expression = await context
            .Expressions.Where(x => x.Id == expressionId)
            .FirstOrDefaultAsync();

        if (expression is null)
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        return new GetExpressionDto()
        {
            Name = expression.Name,
            Id = expression.Id,
            ShortDescription = expression.ShortDescription,
            NavMenuImage = expression.NavMenuImage,
            PublishStatus = (PublishTypes)expression.PublishStatusId,
            PublishTypes = EnumHelpers.GetEnumKeyValuePairs<PublishTypes>(),
        };
    }

    public async Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto)
    {
        var result = await createExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var expression = new Expression()
        {
            Name = dto.Name,
            ShortDescription = dto.ShortDescription,
            NavMenuImage = dto.NavMenuImage,
            PublishStatusId = (int)PublishTypes.Draft,
            ExpressionTypeId = 1, // 1 = expression
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

        var expression = await context.Expressions.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

        if (expression is null)
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        expression.Name = dto.Name;
        expression.ShortDescription = dto.ShortDescription;
        expression.NavMenuImage = dto.NavMenuImage;
        expression.PublishStatusId = (int)dto.PublishStatus;

        context.Expressions.Update(expression);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(expression.Id);
    }

    public async Task<Result> DeleteExpressionAsync(int id)
    {
        var expression = await context
            .Expressions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id && x.ExpressionTypeId == 1); // 1 = expression

        if (expression is null)
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        if (expression.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure(nameof(Expression)));

        expression.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Expression?> GetExpressionForDeletion(int id)
    {
        return await context
            .Expressions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id && x.ExpressionTypeId == 1); // 1 = expression
    }
}
