using ExpressedRealms.Authentication;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Repositories.Shared.Helpers;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using GetExpressionDto = ExpressedRealms.Repositories.Expressions.Expressions.DTOs.GetExpressionDto;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections;

internal sealed class ExpressionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionDtoValidator createExpressionDtoValidator,
    EditExpressionDtoValidator editExpressionDtoValidator,
    IUserContext userContext,
    CancellationToken cancellationToken
) : IExpressionTextSectionRepository
{
    public async Task<Result<List<ExpressionTextSectionNavigationMenuItem>>> GetNavigationMenuItems()
    {
        var canSeeBetaAndDrafts = await userContext.CurrentUserHasPolicy(
            Policies.ExpressionEditorPolicy
        );

        var expression = context.Expressions.AsNoTracking();

        if (!canSeeBetaAndDrafts)
        {
            expression = expression.Where(e => e.PublishStatusId == (int)PublishTypes.Published);
        }

        return await expression
            .Select(x => new ExpressionTextSectionNavigationMenuItem()
            {
                Name = x.Name,
                Id = x.Id,
                ShortDescription = x.ShortDescription,
                NavMenuImage = x.NavMenuImage,
                PublishStatusName = x.PublishStatus.Name,
                PublishStatusId = (PublishTypes)x.PublishStatusId
            })
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<GetExpressionTextSectionDto>> GetExpression(int expressionId)
    {
        var expression = await context
            .Expressions.Where(x => x.Id == expressionId)
            .FirstOrDefaultAsync();

        if (expression is null)
            return Result.Fail(new NotFoundFailure("Expression"));

        return new GetExpressionTextSectionDto()
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
            PublishStatusId = (int)PublishTypes.Draft
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
            return Result.Fail(new NotFoundFailure("Expression"));

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
            .FirstOrDefaultAsync(x => x.Id == id);

        if (expression is null)
            return Result.Fail(new NotFoundFailure("Expression"));

        if (expression.IsDeleted)
            return Result.Fail(new AlreadyDeletedFailure("Expression"));

        expression.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
