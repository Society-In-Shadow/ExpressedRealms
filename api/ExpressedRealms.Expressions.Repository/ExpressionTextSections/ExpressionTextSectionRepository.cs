using ExpressedRealms.DB;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.Helpers;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections;

internal sealed class ExpressionTextSectionRepository(
    ExpressedRealmsDbContext context,
    CreateExpressionTextSectionDtoValidator createExpressionDtoValidator,
    EditExpressionTextSectionDtoValidator editExpressionDtoValidator,
    GetExpressionTestSectionOptionsValidator getExpressionTestSectionOptionsDtoValidator,
    CancellationToken cancellationToken
) : IExpressionTextSectionRepository
{
    public async Task<Result<GetExpressionTextSectionDto>> GetExpressionTextSection(int sectionId)
    {
        var expressionSection = await context.ExpressionSections.FirstOrDefaultAsync(x =>
            x.Id == sectionId
        );

        if (expressionSection is null)
            return Result.Fail(new NotFoundFailure("Expression Section"));

        return new GetExpressionTextSectionDto()
        {
            Id = expressionSection.Id,
            Name = expressionSection.Name,
            Content = expressionSection.Content,
            ParentId = expressionSection.ParentId,
            SectionTypeId = expressionSection.SectionTypeId,
        };
    }

    public async Task<Result<ExpressionTextSectionOptions>> GetExpressionTextSectionOptions(
        GetExpressionTestSectionOptionsDto optionsDto
    )
    {
        var result = await getExpressionTestSectionOptionsDtoValidator.ValidateAsync(
            optionsDto,
            cancellationToken
        );
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var availableParents = await GetParentSectionList(
            optionsDto.ExpressionId,
            optionsDto.SectionId
        );
        var expressSectionTypes = await GetSectionTypes();

        return new ExpressionTextSectionOptions()
        {
            AvailableParents = availableParents,
            ExpressionSectionTypes = expressSectionTypes,
        };
    }

    private async Task<List<SectionTypeDto>> GetSectionTypes()
    {
        return await context
            .ExpressionSectionTypes.Select(x => new SectionTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            })
            .Where(x =>
                x.Name != "Expression"
                && x.Name != "Knowledges Section"
                && x.Name != "Blessings Section"
            )
            .ToListAsync();
    }

    private async Task<List<PotentialParentsDto>> GetParentSectionList(
        int expressionId,
        int? sectionId
    )
    {
        var expressionSections = await context
            .ExpressionSections.Where(x => x.ExpressionId == expressionId)
            .ToListAsync();

        return RecursiveFunctions.GetPotentialParentTargets(expressionSections, null, sectionId);
    }

    public async Task<Result<int>> CreateExpressionTextSectionAsync(
        CreateExpressionTextSectionDto dto
    )
    {
        var result = await createExpressionDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var nextPlaceOnList = await context
            .ExpressionSections.AsNoTracking()
            .Where(x => x.ExpressionId == dto.ExpressionId && x.ParentId == dto.ParentId)
            .CountAsync();

        var expression = new ExpressionSection()
        {
            Name = dto.Name,
            Content = dto.Content,
            ExpressionId = dto.ExpressionId,
            SectionTypeId = dto.SectionTypeId,
            ParentId = dto.ParentId,
            OrderIndex = nextPlaceOnList + 1,
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

        var section = await context
            .ExpressionSections.Where(x => x.Id == dto.Id)
            .FirstOrDefaultAsync();

        if (section is null)
            return Result.Fail(new NotFoundFailure("Expression Section"));

        section.Name = dto.Name;
        section.Content = dto.Content;
        section.ExpressionId = dto.ExpressionId;
        section.SectionTypeId = dto.SectionTypeId;

        context.ExpressionSections.Update(section);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(section.Id);
    }

    public async Task<Result> DeleteExpressionTextSectionAsync(int expressionId, int id)
    {
        var section = await context
            .ExpressionSections.IgnoreQueryFilters()
            .FirstAsync(x => x.ExpressionId == expressionId && x.Id == id);

        section.SoftDelete();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<int>> GetExpressionId(string expressionName)
    {
        var expression = await context.Expressions.FirstOrDefaultAsync(x =>
            x.Name.ToLower() == expressionName.ToLower()
        );

        if (expression is null)
            return Result.Fail(new NotFoundFailure("Expression"));

        return expression.Id;
    }

    public async Task<List<ExpressionSectionDto>> GetExpressionTextSections(int expressionId)
    {
        var sections = await context
            .ExpressionSections.AsNoTracking()
            .Include(x => x.SectionType)
            .Where(x => x.ExpressionId == expressionId && x.SectionType.Name != "Expression")
            .ToListAsync();

        return RecursiveFunctions.BuildExpressionPage(sections, null);
    }

    public async Task<ExpressionSectionDto?> GetExpressionSection(int expressionId)
    {
        var section = await context
            .ExpressionSections.AsNoTracking()
            .Include(x => x.SectionType)
            .FirstOrDefaultAsync(x =>
                x.ExpressionId == expressionId && x.SectionType.Name == "Expression"
            );

        if (section is null)
        {
            return null;
        }

        return new ExpressionSectionDto()
        {
            Name = section.Name,
            Id = section.Id,
            Content = section.Content,
            SectionTypeName = section.SectionType.Name,
        };
    }

    public async Task<ExpressionSection?> GetExpressionSectionForDeletion(
        int expressionId,
        int sectionId
    )
    {
        return await context
            .ExpressionSections.AsNoTracking()
            .IgnoreQueryFilters()
            .Include(x => x.SectionType)
            .FirstOrDefaultAsync(x => x.ExpressionId == expressionId && x.Id == sectionId);
    }

    public async Task<Result> UpdateSectionHierarchyAndSorting(EditExpressionHierarchyDto dto)
    {
        var sections = await context
            .ExpressionSections.Where(x => x.ExpressionId == dto.ExpressionId)
            .ToListAsync();

        foreach (var item in dto.Items)
        {
            var section = sections.First(x => x.Id == item.Id);

            section.ParentId = item.ParentId;
            section.OrderIndex = item.SortOrder;
        }

        await context.SaveChangesAsync();
        return Result.Ok();
    }
}
