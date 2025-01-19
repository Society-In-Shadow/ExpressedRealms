using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Helpers;

public static class ExpressionHelpers
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
}
