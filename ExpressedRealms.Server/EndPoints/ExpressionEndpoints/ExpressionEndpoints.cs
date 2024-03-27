using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints;

internal static class ExpressionEndpoints
{
    internal static void AddExpressionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{name}",
                async (string name, ExpressedRealmsDbContext dbContext) =>
                {
                    var sections = await dbContext
                        .ExpressionSections.AsNoTracking()
                        .Where(x => x.Expression.Name.ToLower() == name.ToLower())
                        .ToListAsync();

                    return TypedResults.Ok(BuildExpressionPage(sections, null));
                }
            )
            .RequireAuthorization();
    }

    private static List<ExpressionSectionDTO> BuildExpressionPage(
        List<ExpressionSection> dbSections,
        int? parentId
    )
    {
        List<ExpressionSectionDTO> sections = new();

        var filteredSections = dbSections
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Id)
            .ToList();
        foreach (var dbSection in filteredSections)
        {
            var dto = new ExpressionSectionDTO()
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
