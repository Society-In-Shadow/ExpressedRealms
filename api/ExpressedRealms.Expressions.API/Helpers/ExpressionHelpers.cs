using ExpressedRealms.Expressions.API.ExpressionEndpoints.UpdateHierarchy;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressionSectionDto = ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs.ExpressionSectionDto;

namespace ExpressedRealms.Expressions.API.Helpers;

internal static class ExpressionHelpers
{
    public static List<ExpressionSectionDto> BuildExpressionPage(
        List<Repository.ExpressionTextSections.DTOs.ExpressionSectionDto> dbSections
    )
    {
        List<ExpressionSectionDto> sections = new();

        foreach (var dbSection in dbSections)
        {
            var dto = new ExpressionSectionDto()
            {
                Name = dbSection.Name,
                Id = dbSection.Id,
                Content = dbSection.Content,
                SectionTypeName = dbSection.SectionTypeName,
            };

            dto.SubSections = BuildExpressionPage(dbSection.SubSections);

            sections.Add(dto);
        }

        return sections;
    }

    public static List<AvailableParentsDto> BuildAvailableParentTree(
        List<PotentialParentsDto> dbSections
    )
    {
        List<AvailableParentsDto> sections = new();

        foreach (var dbSection in dbSections)
        {
            var dto = new AvailableParentsDto() { Name = dbSection.Name, Id = dbSection.Id };

            dto.SubSections = BuildAvailableParentTree(dbSection.SubSections);

            sections.Add(dto);
        }

        return sections;
    }

    public static List<EditExpressionHierarchyItemDto> FlattenHierarchy(
        List<EditExpressionHierarchyItemReqestDto> request
    )
    {
        var flatList = new List<EditExpressionHierarchyItemDto>();

        foreach (var item in request)
        {
            flatList.Add(
                new EditExpressionHierarchyItemDto()
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    SortOrder = item.SortOrder,
                }
            );

            if (item.SubSections.Count == 0)
                continue;
            flatList.AddRange(FlattenHierarchy(item.SubSections));
        }
        return flatList;
    }
}
