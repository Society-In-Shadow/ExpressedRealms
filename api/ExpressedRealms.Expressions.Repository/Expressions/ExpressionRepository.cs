using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Repositories.Shared.Helpers;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using FluentValidationFailure = ExpressedRealms.Repositories.Shared.CommonFailureTypes.FluentValidationFailure;
using NotFoundFailure = ExpressedRealms.Repositories.Shared.CommonFailureTypes.NotFoundFailure;

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
        var allowedStatuses = new List<int>
        {
            (int)PublishTypes.Published
        };

        if (userContext.CurrentUserHasPermission(Permissions.Expression.Edit))
        {
            allowedStatuses.Add((int)PublishTypes.Draft);
        }

        if (userContext.CurrentUserHasPermission(Permissions.Expression.SeeBetaExpressions))
        {
            allowedStatuses.Add((int)PublishTypes.Beta);
        }

        var expression = context.Expressions
            .AsNoTracking()
            .Where(e => allowedStatuses.Contains(e.PublishStatusId));

        return await expression
            .Select(x => new ExpressionNavigationMenuItem()
            {
                ExpressionTypeId = x.ExpressionTypeId,
                Name = x.Name,
                Id = x.Id,
                ShortDescription = x.ShortDescription,
                NavMenuImage = x.NavMenuImage,
                PublishStatusName = x.PublishStatus.Name,
                PublishStatusId = (PublishTypes)x.PublishStatusId,
                OrderIndex = x.OrderIndex,
            })
            .OrderBy(x => x.OrderIndex)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<GetExpressionDto>> GetExpression(int expressionId)
    {
        var results = await CheckUserPermissionsForExpressionEdit(expressionId);

        if (!results.IsSuccess)
            return results;
        
        var expression = await context
            .Expressions.Where(x => x.Id == expressionId)
            .Select(expression => new 
            {
                expression.Name,
                expression.Id,
                expression.ShortDescription,
                expression.NavMenuImage,
                expression.PublishStatusId,
                expression.OrderIndex,
                expression.ExpressionTypeId,
            })
            .FirstOrDefaultAsync();
        
        return new GetExpressionDto
        {
            Name = expression.Name,
            Id = expression.Id,
            ShortDescription = expression.ShortDescription,
            NavMenuImage = expression.NavMenuImage,
            PublishStatus = (PublishTypes)expression.PublishStatusId,
            PublishTypes = EnumHelpers.GetEnumKeyValuePairs<PublishTypes>(),
            OrderIndex = expression.OrderIndex
        };
    }

    private async Task<Result<GetExpressionDto>> CheckUserPermissionsForExpressionEdit(int expressionId)
    {
        var expressionTypeLookup = await context.Expressions.AsNoTracking()
            .Where(x => x.Id == expressionId)
            .Select(x => x.ExpressionTypeId)
            .FirstOrDefaultAsync();

        if(expressionTypeLookup == 1 && !userContext.CurrentUserHasPermission(Permissions.Expression.View))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        var cmsTypes = new List<int> { 13, 14 };
        if(cmsTypes.Contains(expressionTypeLookup) && !userContext.CurrentUserHasPermission(Permissions.ContentManagementSystem.View))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));
        
        if (expressionTypeLookup == 0)
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        return Result.Ok();
    }
    
    private async Task<Result<GetExpressionDto>> CheckUserPermissionsForExpressionDelete(int expressionId)
    {
        var expressionTypeLookup = await context.Expressions.AsNoTracking()
            .Where(x => x.Id == expressionId)
            .Select(x => x.ExpressionTypeId)
            .FirstOrDefaultAsync();

        if(expressionTypeLookup == 1 && !userContext.CurrentUserHasPermission(Permissions.Expression.Delete))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        var cmsTypes = new List<int> { 13, 14 };
        if(cmsTypes.Contains(expressionTypeLookup) && !userContext.CurrentUserHasPermission(Permissions.ContentManagementSystem.Delete))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));
        
        if (expressionTypeLookup == 0)
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        return Result.Ok();
    }

    private Result<GetExpressionDto> ExpressionTypePermissionCheck(int expressionTypeLookup)
    {
        if(expressionTypeLookup == 1 && !userContext.CurrentUserHasPermission(Permissions.Expression.Create))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        var cmsTypes = new List<int> { 13, 14 };
        if(cmsTypes.Contains(expressionTypeLookup) && !userContext.CurrentUserHasPermission(Permissions.ContentManagementSystem.Create))
            return Result.Fail(new NotFoundFailure(nameof(Expression)));

        return Result.Ok();
    }

    public async Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto)
    {
        var result = await createExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var authResult = ExpressionTypePermissionCheck(dto.ExpressionTypeId);
        if(authResult.IsFailed)
            return authResult.ToResult();
        
        var maxSort = await context
            .Expressions.Where(x => x.ExpressionTypeId == dto.ExpressionTypeId)
            .MaxAsync(x => x.OrderIndex, cancellationToken);

        var expression = new Expression()
        {
            Name = dto.Name,
            ShortDescription = dto.ShortDescription,
            NavMenuImage = dto.NavMenuImage,
            PublishStatusId = (int)PublishTypes.Draft,
            ExpressionTypeId = dto.ExpressionTypeId,
            OrderIndex = maxSort + 1,
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
        
        var authResult = await CheckUserPermissionsForExpressionEdit(dto.Id);
        if(authResult.IsFailed)
            return authResult.ToResult();
        
        var expression = await context.Expressions.Where(x => x.Id == dto.Id).FirstAsync();
        
        expression.Name = dto.Name;
        expression.ShortDescription = dto.ShortDescription;
        expression.NavMenuImage = dto.NavMenuImage;
        expression.PublishStatusId = (int)dto.PublishStatus;

        var sections = await context
            .Expressions.Where(x => x.ExpressionTypeId == expression.ExpressionTypeId)
            .OrderBy(x => x.OrderIndex)
            .ToListAsync();

        // Make sure they can't go crazy high
        if (dto.SortOrder > sections.Count)
        {
            dto.SortOrder = sections.Count;
        }

        var index = 1;
        foreach (var item in sections)
        {
            // Leave hole in order for new position
            if (item.OrderIndex == dto.SortOrder)
            {
                index++;
            }

            item.OrderIndex = item.Id == dto.Id ? dto.SortOrder : index;

            index++;
        }

        context.Expressions.Update(expression);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(expression.Id);
    }

    public async Task<Result> DeleteExpressionAsync(int id)
    {
        var authResult = await CheckUserPermissionsForExpressionDelete(id);
        if(authResult.IsFailed)
            return authResult.ToResult();
        
        var expression = await context.Expressions
            .FirstAsync(x => x.Id == id);

        expression.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Expression?> GetExpressionForDeletion(int id)
    {
        return await context.Expressions.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Expression?> ExpressionExists(int id)
    {
        return await context.Expressions.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExpressionExistsForModifiers(int id)
    {
        return await context.Expressions.AnyAsync(x => x.Id == id && x.ExpressionTypeId == 1);
    }

    public async Task<bool> ExpressionTypeExists(int id)
    {
        return await context.ExpressionTypes.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Expression>> GetAllEnabledExpressions()
    {
        return await context.Expressions.Where(x => x.ExpressionTypeId == 1).ToListAsync(); // 1 = expression
    }
}
