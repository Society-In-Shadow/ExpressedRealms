using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.Helpers;

public static class RecursiveFunctions
{
    public static List<ExpressionSectionDto> BuildExpressionPage(
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
    
    public static List<ExpressionSectionDto> GetPotentialParentTargets(
        List<ExpressionSection> dbSections,
        int? parentId,
        int? excludedChildrenId
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

            if (dbSections.Any(x => x.ParentId == dbSection.Id) && dbSection.Id != excludedChildrenId)
            {
                dto.SubSections = GetPotentialParentTargets(dbSections, dbSection.Id, excludedChildrenId);
            }

            sections.Add(dto);
        }

        return sections;
    }
    
    public static List<int> GetValidParentIds(
        List<ExpressionSection> dbSections,
        int? parentId,
        int excludedChildrenId
    )
    {
        List<int> sections = new();

        var filteredSections = dbSections
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Id)
            .ToList();
        
        foreach (var dbSection in filteredSections)
        {
            if (dbSections.Any(x => x.ParentId == dbSection.Id) && dbSection.Id != excludedChildrenId)
            {
                sections.AddRange(GetValidParentIds(dbSections, dbSection.Id, excludedChildrenId));
            }

            sections.Add(dbSection.Id);
        }

        return sections;
    }
}