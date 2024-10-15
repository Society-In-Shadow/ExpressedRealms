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

internal sealed class ExpressionTextSectionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionTextSectionDtoValidator createExpressionDtoValidator,
    EditExpressionTextSectionDtoValidator editExpressionDtoValidator,
    IUserContext userContext,
    CancellationToken cancellationToken
) : IExpressionTextSectionRepository
{
    public async Task<Result<GetExpressionTextSectionDto>> GetExpressionTextSection(int expressionId)
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

    public async Task<Result<int>> CreateExpressionTextSectionAsync(CreateExpressionTextSectionDto dto)
    {
        var result = await createExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var expression = new ExpressionSection()
        {
            Name = dto.Name,
            Content = dto.Content,
            ExpressionId = dto.ExpressionId,
            SectionTypeId = dto.SectionTypeId
        };

        context.ExpressionSections.Add(expression);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(expression.Id);
    }

    public async Task<Result<int>> EditExpressionTextSectionAsync(EditExpressionTextSectionDto dto)
    {
        var result = await editExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var section = await context.ExpressionSections.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

        if (section is null)
            return Result.Fail(new NotFoundFailure("Expression"));

        section.Name = dto.Name;
        section.Content = dto.Content;
        section.ExpressionId = dto.ExpressionId;
        section.SectionTypeId = dto.SectionTypeId;

        context.ExpressionSections.Update(section);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(section.Id);
    }

    public async Task<Result> DeleteExpressionTextSectionAsync(int id)
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

    public async Task<List<ExpressionSectionDto>> GetExpressionTextSections(int expressionId)
    {
        var sections = await context
            .ExpressionSections.AsNoTracking()
            .Where(x => x.ExpressionId == expressionId)
            .ToListAsync();

        return BuildExpressionPage(sections, null);
    }
    
    private static List<ExpressionSectionDto> BuildExpressionPage(
        List<ExpressionSection> dbSections,
        int? parentId
    )
    {
        List<ExpressionSectionDto> sections = new();

        var filteredSections = dbSections
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Id)
            .ToList();
        foreach (var dbSection in filteredSections)
        {
            var dto = new ExpressionSectionDto()
            {
                Name = dbSection.Name,
                Id = dbSection.Id,
                Content = dbSection.Content,
            };

            if (dbSections.Any(x => x.ParentId == dbSection.Id))
            {
                dto.SubSections = BuildExpressionPage(dbSections, dbSection.Id);
            }

            sections.Add(dto);
        }

        return sections;
    }
}
