using ExpressedRealms.Expressions.API.ExpressionEndpoints.DTOs;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.Requests;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.Helpers;

internal static class ExpressionHelpers
{
    public static List<ExpressionSectionDTO> BuildExpressionPage(
        List<ExpressionSectionDto> dbSections
    )
    {
        List<ExpressionSectionDTO> sections = new();

        foreach (var dbSection in dbSections)
        {
            var dto = new ExpressionSectionDTO()
            {
                Name = dbSection.Name,
                Id = dbSection.Id,
                Content = dbSection.Content,
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
