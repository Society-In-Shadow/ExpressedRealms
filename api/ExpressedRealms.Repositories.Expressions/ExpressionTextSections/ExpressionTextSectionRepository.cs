using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.Helpers;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections;

internal sealed class ExpressionTextSectionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionTextSectionDtoValidator createExpressionDtoValidator,
    EditExpressionTextSectionDtoValidator editExpressionDtoValidator,
    CancellationToken cancellationToken
) : IExpressionTextSectionRepository
{
    public async Task<Result<GetExpressionTextSectionDto>> GetExpressionTextSection(int sectionId)
    {
        var expressionSection = await context.ExpressionSections
            .FirstOrDefaultAsync(x => x.SectionTypeId == sectionId);

        if (expressionSection is null)
            return Result.Fail(new NotFoundFailure("Expression Section"));
        
        var expressionSections = await context.ExpressionSections
            .Where(x => x.ExpressionId == expressionSection.ExpressionId)
            .ToListAsync();
        
        var availableParents = RecursiveFunctions.GetPotentialParentTargets(expressionSections, null, sectionId);
        
        var expressSectionTypes = await context
            .ExpressionSectionTypes
            .Select(x => new SectionTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .ToListAsync();

        return new GetExpressionTextSectionDto()
        {
            Id = expressionSection.Id,
            Name = expressionSection.Name,
            Content = expressionSection.Content,
            ParentId = expressionSection.ParentId,
            AvailableParents = availableParents,
            SectionTypeId = expressionSection.SectionTypeId,
            SectionTypes = expressSectionTypes,
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
            .ExpressionSections.IgnoreQueryFilters()
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

        return RecursiveFunctions.BuildExpressionPage(sections, null);
    }
}
